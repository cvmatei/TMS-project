using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;
using TMS_Project.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TMS_Project
{
    public class Buyer
    {
        Contract_Marketplace conn;
        const int MAX_STRINGS = 10;
        string query;




        /// <summary>
        /// Makes new contracts for the contract table.
        /// </summary>
        /// <returns>string array of new customers</returns>
        public string[] getNewContracts()
        {
            conn = new Contract_Marketplace();
            TMS_Data data = new TMS_Data();
            String[] contracts = new String[MAX_STRINGS];
            string[] newCustomers = new string[MAX_STRINGS];
            DataTable customerTable = new DataTable();
            DataTable contractTable = new DataTable();
            bool exist = false;
            int i = 0;
            

            contracts = conn.ConnectDB();

            //Gets the entire customer table.
            query = "select * from customer";
            customerTable = data.SelectTMS(query);


            try
            {
                data.OpenConnection();

                //Iterates through the contracts and parses the info.
                foreach (string customerName in contracts)
                {

                    //If the name isn't empty, proceed.
                    if (customerName != null)
                    {
                        string[] contractInfo = customerName.Split(';');


                        //Compares the customer on the contract with the customer table to see if they already exist.
                        foreach(DataRow row in customerTable.Rows)
                        {
                            if (contractInfo[0] == row.Field<string>("customer_name"))
                            {
                                exist = true;
                            }
                        }

                        
                        //If the customer already exists, they will be inserted into the contract table.
                        if(exist)
                        {
                            uint customerID = 0;

                            //Gets the ID that corresponds with the customer name.
                            foreach (DataRow row in customerTable.Rows)
                            {
                                if (contractInfo[0] == row.Field<string>("customer_name"))
                                {
                                    customerID = row.Field<uint>("customer_ID");
                                }
                            }

                            query = "select * from contract;";
                            contractTable = data.SelectTMS(query);
                            bool contractExist = false;


                            //Iterates through the contract table to see if the exact same contract already exists. No duplicates are saved.
                            foreach(DataRow row in contractTable.Rows)
                            {
                                if(row.Field<uint>("customer_ID") == customerID && 
                                   row.Field<int>("job_type") == Convert.ToInt16(contractInfo[1]) &&
                                   row.Field<int>("quantity") == Convert.ToInt16(contractInfo[2]) &&
                                   row.Field<int>("van_type") == Convert.ToInt16(contractInfo[5]) &&
                                   row.Field<string>("origin_city") == contractInfo[3] &&
                                   row.Field<string>("destination_city") == contractInfo[4])
                                {
                                    contractExist = true;
                                }
                            }


                            //If this is a new contract, it will be inserted into the table.
                            if (!contractExist)
                            {


                                //Inserts all contract information into the contract table.
                                query = "insert into contract (customer_ID, job_type, quantity, van_type, origin_city, destination_city) values " + "(\'" + customerID + "\', \'"
                                    + contractInfo[1] + "\', \'" + contractInfo[2] + "\', \'" + contractInfo[5] + "\', \'" + contractInfo[3] + "\', \'" + contractInfo[4] + "\');";

                                data.InsertTMS(query);
                            }

                        }
                        else
                        {
                            bool duplicate = false;

                            //Checks if the customer already exists within the customer table.
                            foreach(string customer in newCustomers)
                            {
                                if (customer == contractInfo[0])
                                {
                                    duplicate = true;
                                }
                            }

                            //Customer names will be added to the array if they are new.
                            if(!duplicate)
                            {
                                newCustomers[i] = contractInfo[0];
                            }
                            
                            //Iterates the new customer array.
                            i++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                BuyerLogger.fatalBuyer("Connection error.");
            }

            data.CloseConnection();

            return newCustomers;
        }



        /// <summary>
        /// Adds a new customer to the table.
        /// </summary>
        /// <param name="name"></param>
        public void acceptNewCustomers(string name)
        {            
            TMS_Data data = new TMS_Data();
            data.OpenConnection();

            //Inserts the customer into the table.
            query = "insert into customer (customer_name) values (\'" + name + "\');";
            data.InsertTMS(query);

            data.CloseConnection();

        }




        /// <summary>
        /// This function calculates the trip distance, duration, finds an appropriate carrier,
        /// and inserts all data into the trips and orders tables.
        /// </summary>
        /// <param name="contractID"></param>
        public void newOrder(uint contractID)
        {
            TMS_Data data = new TMS_Data();
            String[] cityInfo = { "Windsor 191 2.5", "London 128 1.75", "Hamilton 68 1.25", "Toronto 60 1.3", "Oshawa 134 1.65", "Belleville 82 1.2", "Kingston 196 2.5", "Ottawa" };
            String[] temp = null;
            DataTable response;
            
            double duration = 0;
            string origin = null;
            string destination = null;
            int distance = 0;
            int cargo = 0;
            uint carrier_id = 0;
            bool backwards = false;
            bool calcTrip = false;


            try
            {
                data.OpenConnection();




                //Selects all information for a specified contract.
                query = "select * from contract where contract_ID = " + contractID + ";";
                response = data.SelectTMS(query);



                foreach (DataRow row in response.Rows)
                {
                    origin = row.Field<string>("origin_city");
                    destination = row.Field<string>("destination_city");
                    cargo = row.Field<int>("van_type");
                }




                //Iterates through the string array and calculates the distance and duration of the trip.
                foreach (string city in cityInfo)
                {
                    temp = city.Split(' ');


                    //Checks if the origin is equal to the current iteration.
                    if (temp[0] == origin)
                    {

                        //The trip can start calculating.
                        calcTrip = true;


                        //Checks if the trip has been calculating backwards.
                        if (backwards)
                        {
                            break;
                        }

                        distance = Convert.ToInt32(temp[1]);
                        duration = Convert.ToDouble(temp[2]);
                        continue;
                    }
                    else if (temp[0] == destination)
                    {

                        //Checks if the trip needs to be calculated backwards.
                        if (!calcTrip)
                        {
                            backwards = true;
                            calcTrip = true;

                            distance = Convert.ToInt32(temp[1]);
                            duration = Convert.ToDouble(temp[2]);
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }



                    if (calcTrip)
                    {
                        distance += Convert.ToInt32(temp[1]);
                        duration += Convert.ToDouble(temp[2]);
                    }
                }


                //Inserts all data into the orders table.
                query = "insert into orders (contract_ID, carrier_ID, trip_ID, cargo_type, origin_city, trip_duration, completed) values " + "(" + contractID + ", NULL, NULL, " + cargo + ", \'" + origin + "\', " + duration + ", NULL );";
                data.InsertTMS(query);

                data.CloseConnection();
            }
            catch(Exception)
            {
                BuyerLogger.warnBuyer("Could not connect to database.");
            }
        }




        /// <summary>
        /// This function get new customers from the contract marketplace.
        /// </summary>
        /// <returns>An array of customers.</returns>
        public String[] noCustomers()
        {
            conn = new Contract_Marketplace();
            String[] parse = conn.ConnectDB();
            String[] temp = new string[MAX_STRINGS];
            String[] customers = new string[MAX_STRINGS];
            int i = 0;
            bool duplicate = false;


            //Parses the string to leave just the customer name, avoiding duplicates.
            foreach (String str in parse) 
            {
                if(str != null)
                {
                    temp = str.Split(';');


                    for(int j = 0; j < customers.Length; j++)
                    {
                        if (customers[j] == temp[0])
                        {
                            duplicate = true;
                        }
                    }


                    if(!duplicate)
                    {
                        customers[i] = temp[0];
                    }
                    
                    i++;
                }
            }

            
            return customers;
        }





        /// <summary>
        /// Generates an invoice for an order by inserting the information into
        /// the database and as a text file.
        /// </summary>
        /// <param name="orderID"></param>
        public void generateInvoice(uint orderID)
        {
            TMS_Data data = new TMS_Data();
            int length = 0;
            int reefer = 0;
            int jobType = 0;
            int pallets = 0;
            uint carrierID = 0;
            uint contractID = 0;
            uint customerID = 0;
            uint tripID = 0;
            float rate = 0;
            float FTLRate = 0;
            float LTLRate = 0;
            float reeferRate = 0;
            string customer = null;
            string invoiceName = null;
            string carrierName = null;
            DateTime theDate = DateTime.Now;
            var date = theDate.ToString("yyyy-MM-dd");


            try
            {
                data.OpenConnection();
                DataTable response = new DataTable();

                //Gets the tripID.
                query = "select * from orders where order_ID = " + orderID + ";";
                response = data.SelectTMS(query);
                tripID = response.Rows[0].Field<uint>("trip_ID");
                contractID = response.Rows[0].Field<uint>("contract_ID");



                //Gets the contract information.
                query = "select * from contract where contract_ID = " + contractID + ";";
                response = data.SelectTMS(query);

                foreach (DataRow row in response.Rows)
                {
                    customerID = row.Field<uint>("customer_ID");
                    contractID = row.Field<uint>("contract_ID");
                    jobType = row.Field<int>("job_type");
                    pallets = row.Field<int>("quantity");
                }


                //Gets the customer.
                query = "select * from customer where customer_ID = " + customerID + ";";
                response = data.SelectTMS(query);

                foreach (DataRow row in response.Rows)
                {
                    customer = row.Field<string>("customer_name");
                }


                //Gets the order information.
                query = "select * from orders where trip_ID = " + tripID + ";";
                response = data.SelectTMS(query);

                foreach (DataRow row in response.Rows)
                {
                    reefer = row.Field<int>("cargo_type");
                    carrierID = row.Field<uint>("carrier_ID");
                }


                //Gets the distance.
                query = "select * from trips where trip_ID = " + tripID + ";";
                response = data.SelectTMS(query);

                foreach (DataRow row in response.Rows)
                {
                    length = row.Field<int>("trip_length");
                }


                //Gets the carrier information.
                query = "select * from carriers where carrier_ID = " + carrierID + ";";
                response = data.SelectTMS(query);

                foreach (DataRow row in response.Rows)
                {
                    carrierName = row.Field<string>("carrier_name");
                    FTLRate = row.Field<float>("ftl_rate");
                    LTLRate = row.Field<float>("ltl_rate");
                    reeferRate = row.Field<float>("reefer_charge");
                }





                //Calculates the rate, checks what type of job.
                if (jobType == 0)
                {
                    rate = length * FTLRate;
                }
                else
                {
                    if (LTLRate != 0)
                    {
                        rate = (length * LTLRate) * pallets;
                    }
                    else
                    {
                        rate = length * pallets;
                    }
                }

                float reeferCost = 0;
                if (reefer == 1)
                {
                    reeferCost = rate * reeferRate;
                    rate = rate + reeferCost;
                }


                //Multiplied by 100 to display a percentage in the invoice.
                reeferRate = reeferRate * 100;


                //Names the text file after the customer and contract ID.
                invoiceName = customer + contractID + ".txt";

                string invoiceContent = null;


                //Outputs one of two invoices depending on the job type.
                if (jobType == 0)
                {
                    invoiceContent =
                        "Omnicorp Shipping Handling and Transportation\n\n" +
                        "Invoice ID:\t" + contractID + "\n" +
                        "Issue Date:\t" + theDate + "\n" +
                        "Customer:\t" + customer + "\n\n\n" +
                        "Carrier:\t\t\t" + carrierName + "\n" +
                        "Full Truck Load Rate:\t\t$" + FTLRate + "/km\n" +
                        "Distance:\t\t\t" + length + "km\n" +
                        "Shipping Rate:\t\t\t$" + Math.Round((rate - reeferCost), 2) + "\n" +
                        +reeferRate + "% Reefer Charge:\t\t" + "$" + Math.Round(reeferCost, 2) + "\n" +
                        "--------------------------------------------------------------------------------------------------\n" +
                        "Total:\t\t\t\t$" + Math.Round(rate, 2);
                }
                else
                {
                    invoiceContent =
                        "Omnicorp Shipping Handling and Transportation\n\n" +
                        "Invoice ID:\t" + contractID + "\n" +
                        "Issue Date:\t" + theDate + "\n" +
                        "Customer:\t" + customer + "\n\n\n" +
                        "Carrier:\t\t\t" + carrierName + "\n" +
                        "Less Than Load Rate:\t\t$" + LTLRate + "/km for each pallet\n" +
                        "# of Pallets:\t\t\t" + pallets + "\n" +
                        "Distance:\t\t\t" + length + "km\n" +
                        "Shipping Rate:\t\t\t$" + Math.Round((rate - reeferCost), 2) + "\n" +
                        +reeferRate + "% Reefer Charge:\t\t" + "$" + Math.Round(reeferCost, 2) + "\n" +
                        "--------------------------------------------------------------------------------------------------\n" +
                        "Total:\t\t\t\t$" + Math.Round(rate, 2);
                }


                try
                {
                    using (StreamWriter sw = File.CreateText(invoiceName))
                    {
                        sw.Write(invoiceContent);
                    }
                }
                catch (Exception)
                {
                    BuyerLogger.warnBuyer("Invoice generation failed.");
                }


                //Inserts the invoice into the database.
                query = "insert into invoices (customer_ID, date, carrier_ID, job_type, van_type, quantity, distance, total) values " +
                        "(" + customerID + ", \'" + date + "\', " + carrierID + ", " + jobType + ", " + reefer + ", " + pallets + ", " + length + ", " + rate + ");";

                data.InsertTMS(query);


                query = "update orders set completed = \'done\' where order_ID = " + orderID + ";";
                data.UpdateTMS(query);


                data.CloseConnection();
            }
            catch(Exception) 
            {
                BuyerLogger.warnBuyer("Error occured generating invoice.");
            }
        }
    }
}
