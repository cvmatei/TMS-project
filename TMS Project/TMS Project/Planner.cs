using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMS_Project.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TMS_Project
{
    internal class Planner
    {
        string query;
        TMS_Data conn = new TMS_Data();
        DataTable data = new DataTable();
        
        public string[] receiveOrder()
        {
            string[] orders = new string[10];
            string info;
            int i = 0;

            conn.OpenConnection();
            query = "select * from orders where completed is NULL;";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach (DataRow row in data.Rows)
            {
                info = "ID: " + row.Field<uint>("order_ID") + " ";
                info += "Origin City: " + row.Field<String>("origin_city");
                orders[i++] = info;
            }
            return orders;
        }

        public string[] activeOrder()
        {
            int i = 0;
            string[] active = new string[10];

            conn.OpenConnection();
            query = "select * from orders where completed = \'false\'";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach(DataRow row in data.Rows)
            {
                active[i++] = "ID: " + row.Field<uint>("order_ID").ToString()
                    + " CarrierID: " + row.Field<uint>("carrier_ID").ToString()
                    + " TripID: " + row.Field<uint>("trip_ID").ToString()
                    + " Trip Duration: " + row.Field<double>("trip_duration").ToString()
                    + " Completed: " + row.Field<string>("completed");
            }
            return active;
        }

        public string[] completeOrder()
        {
            int i = 0;
            string[] complete = new string[10];

            conn.OpenConnection();
            query = "select * from orders where completed = \'true\'";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach (DataRow row in data.Rows)
            {
                complete[i++] = "ID: " + row.Field<uint>("order_ID").ToString()
                    + " CarrierID: " + row.Field<uint>("carrier_ID").ToString()
                    + " TripID: " + row.Field<uint>("trip_ID").ToString()
                    + " Duration: " + row.Field<double>("trip_duration").ToString()
                    + " Completed: " + row.Field<string>("completed");
            }
            return complete;
        }

        /// <summary>
        /// This function shows all the carriers possible for a specific order
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns>
        /// A DataTable that shows all carriers that match the orders origin city
        /// </returns>
        public string[] showCarriers(string city)
        {
            string[] carriers = new string[10];
            int i = 0;

            conn.OpenConnection();
            query = "select carrier_ID, carrier_name from carriers where depot_city = \'" + city + "\';";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach(DataRow row in data.Rows)
            {
                carriers[i++] = "ID: " + row.Field<uint>("carrier_ID") + " " + row.Field<string>("carrier_name");
            }

            return carriers;
        }

        /// <summary>
        /// This function updates an order to have a specific carrier
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="carrier_id"></param>
        /// <returns"></returns>
        public void selectCarrier(int order_id, int carrier_id)
        {
            bool calcTrip = false;
            bool backwards = false;
            uint trip_id = 0;
            uint contractID = 0;
            double duration = 0;
            int distance = 0;
            int cargo = 0;
            string origin = "";
            string destination = "";
            string[] temp;
            String[] cityInfo = { "Windsor 191 2.5", "London 128 1.75", "Hamilton 68 1.25", "Toronto 60 1.3", "Oshawa 134 1.65", "Belleville 82 1.2", "Kingston 196 2.5", "Ottawa" };

            conn.OpenConnection();
            query = "update orders set carrier_ID = " + carrier_id + " where order_ID = " + order_id + ";";
            conn.UpdateTMS(query);
            conn.CloseConnection();

            conn.OpenConnection();
            query = "update orders set completed = \'false\' where order_id = " + order_id + ";";
            conn.UpdateTMS(query);
            conn.CloseConnection();

            conn.OpenConnection();
            query = "select * from orders where order_id = " + order_id + ";";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach(DataRow row in data.Rows)
            {
                duration = row.Field<double>("trip_duration");
                origin = row.Field<string>("origin_city");
                contractID = row.Field<uint>("contract_ID");
            }

            //Selects all information for a specified contract.
            conn.OpenConnection();
            query = "select * from contract where contract_ID = " + contractID + ";";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach (DataRow row in data.Rows)
            {
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
                }
            }

            conn.OpenConnection();
            query = "insert into trips (destination_city, trip_length, trip_duration) values (\'" + origin + "\', " + distance + ", " + duration + ");";
            conn.InsertTMS(query);
            conn.CloseConnection();

            conn.OpenConnection();
            query = "select * from trips where trip_duration = " + duration + ";";
            data = conn.SelectTMS(query);
            conn.CloseConnection();

            foreach(DataRow row in data.Rows)
            {
                trip_id = row.Field<uint>("trip_ID");
            }

            conn.OpenConnection();
            query = "update orders set trip_ID = " + trip_id + " where order_ID = " + order_id + ";";
            conn.UpdateTMS(query);
            conn.CloseConnection();
        }

        /// <summary>
        /// This function updates an order to have a specific carrier
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns>
        /// A value for time as days
        /// </returns>
        public int simulateTimePassage(int order_id)
        {
            int time;
            double duration = 0;

            conn.OpenConnection();
            query = "select * from orders where order_ID = " + order_id + ";";
            data = conn.SelectTMS(query);
            conn.CloseConnection();
            
            foreach(DataRow row in data.Rows)
            {
                duration = row.Field<double>("trip_duration");
            }

            if(duration > 8)
            {
                if(duration > 16)
                {
                    time = 3;
                }
                else
                {
                    time = 2;
                }
            }
            else
            {
                time = 1;
            }
            return time;
        }

        /// <summary>
        /// This function updates an order to be completed
        /// </summary>
        /// <param name="order_id"></param>
        /// <returns></returns>
        public void confirmOrder(int order_id)
        {
            conn.OpenConnection();
            query = "update orders set completed = \'true\' where order_ID = " + order_id + ";";
            conn.UpdateTMS(query);
            conn.CloseConnection();
        }

        /// <summary>
        /// This function gets either 2 week old invoices or all of them and creates a summary string to display
        /// </summary>
        /// <param name="weeks"></param>
        /// <returns>
        /// A string that shows a summary of the invoice data with the total earnings at the bottom
        /// </returns>
        public string generateSummary(int weeks)
        {
            string summary = "Invoice Summary\n\n";
            decimal total = 0;

            conn.OpenConnection();
            if(weeks == 0)
            {
                query = "select * from invoices;";
                data = conn.SelectTMS(query);
            }
            else
            {
                DateTime twoWeeksAgo = DateTime.Now.AddDays(-14);
                query = "select * from invoices where date >= \'" + twoWeeksAgo.ToShortDateString() + "\';";
                data = conn.SelectTMS(query);
            }
            
            foreach (DataRow row in data.Rows)
            {
                total += row.Field<decimal>("total"); 
            }
            conn.CloseConnection();

            foreach(DataRow row in data.Rows)
            {
                summary += "InvoiceID: " + row.Field<uint>("invoice_ID").ToString() + " ";
                summary += "CustomerID: " + row.Field<uint>("customer_ID").ToString() + " ";
                summary += "Date: " + row.Field<DateTime>("date") + " ";
                summary += "CarrierID: " + row.Field<uint>("carrier_ID").ToString() + " ";
                summary += "Job-Type: " + row.Field<int>("job_type").ToString() + " ";
                summary += "Van-Type: " + row.Field<int>("van_type").ToString() + " ";
                summary += "Quantity: " + row.Field<int>("quantity").ToString() + " ";
                summary += "Distance: " + row.Field<int>("distance").ToString() + " ";
                summary += "Total: " + row.Field<decimal>("total").ToString() + "\n\n";
            }
            summary += "TOTAL EARNINGS: $" + total.ToString();

            string summaryName = "Invoice Summary " + DateTime.Now.ToString() + ".txt";

            try
            {
                using (StreamWriter sw = File.CreateText(summaryName))
                {
                    sw.Write(summary);
                }
            }
            catch (Exception)
            {
                PlannerLogger.warnPlanner("Invoice Summary Generation Failed.");
            }

            return summary;
        }
    }
}
