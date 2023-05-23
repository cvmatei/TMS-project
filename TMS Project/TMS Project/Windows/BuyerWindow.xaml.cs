using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMS_Project.Logging;

namespace TMS_Project
{
    /// <summary>
    /// Interaction logic for BuyerWindow.xaml
    /// </summary>
    public partial class BuyerWindow : Window
    {
        bool prevCustomers;
        uint contractID;
        uint orderID;


        public BuyerWindow()
        {
            InitializeComponent();
            prevCustomers = false;
            contractID = 0;
            orderID = 0;
        }


        private void HomeButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton(object sender, RoutedEventArgs e)
        {
            Buyer buyer = new Buyer();


            //If there havent been any previous customers yet, a new set of customers will be displayed.
            if(!prevCustomers)
            {
                AcceptButtonBuyerScreen.Content = "Accept";
                String[] customers = new string[10]; 
                customers = buyer.noCustomers();
                

                //Lists the new customers.
                foreach(string customer in customers)
                {
                    if(customer != null)
                    {
                        CustomerListBox.Items.Add(customer);
                    }
                    
                }
                prevCustomers = true;
            }
            else
            {
                //If the button has been clicked at least once, the button now checks for selected items.
                if(CustomerListBox.SelectedItem != null)
                {
                    string name = CustomerListBox.SelectedItem.ToString();
                    buyer.acceptNewCustomers(name);
                    CustomerListBox.Items.Remove(name);
                }
            }
        }

        private void SubmitToPlannerButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if(NewOrdersListBox.SelectedItem != null)
                {
                    Buyer buyer = new Buyer();
                    buyer.newOrder(contractID);
                    NewOrdersListBox.Items.Remove(NewOrdersListBox.SelectedItem.ToString());
                }
            }
            catch(Exception)
            {
                BuyerLogger.warnBuyer("Unable to send item to planner.");
            }

        }

        private void GenerateInvoiceButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if(CompletedOrdersListBox.SelectedItem != null)
                {
                    Buyer buyer = new Buyer();
                    buyer.generateInvoice(orderID); 
                    CompletedOrdersListBox.Items.Remove(CompletedOrdersListBox.SelectedItem.ToString());
                }
            }
            catch(Exception) 
            {
                BuyerLogger.warnBuyer("Unaable to generate invoice for this item.");
            }

        }

        private void CustomerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ExistButton(object sender, RoutedEventArgs e)
        {
            TMS_Data data = new TMS_Data();
            DataTable customerTable = new DataTable();
            try 
            { 
                data.OpenConnection();


                string message = "select * from customer";
                customerTable = data.SelectTMS(message);


                //Displays a messagebox of all the existing customers.
                message = "";
                if (customerTable.Rows.Count == 0)
                {
                    MessageBox.Show("Currently no customers.");
                }
                else
                {
                    foreach(DataRow row in customerTable.Rows)
                    {
                        message += row.Field<string>("customer_name") + "\n";
                    }
                    MessageBox.Show(message);
                }

                data.CloseConnection();
            }
            catch(Exception)
            {
                BuyerLogger.warnBuyer("Unable to dispay existing customers.");
            }
        }

        private void SelectCityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GetContractsButton(object sender, RoutedEventArgs e)
        {
            Buyer buyer = new Buyer();
            TMS_Data data = new TMS_Data();
            DataTable contractTable = new DataTable();
            DataTable customerTable = new DataTable();
            string[] newCustomers = new string[10];
            



            try
            {           
                newCustomers = buyer.getNewContracts();


                //Updates the new customer list.
                if (newCustomers[0] != null)
                {
                    foreach (string customer in newCustomers)
                    {
                        if (customer != null && !CustomerListBox.Items.Contains(customer))
                        {
                            CustomerListBox.Items.Add(customer);
                        }

                    }
                }


                data.OpenConnection();


                string query = "select * from contract;";
                contractTable = data.SelectTMS(query);


                //Parses the data from the table to make a cohesive string in the listbox.
                foreach(DataRow row in contractTable.Rows) 
                {
                    string addContract = "ID: " + row.Field<uint>("contract_ID") + " | Customer: ";


                    query = "select * from customer where customer_ID = " + row.Field<uint>("customer_ID") + ";";
                    customerTable = data.SelectTMS(query);

                    addContract += customerTable.Rows[0].Field<string>("customer_name");


                    //Depending on the job type, the string will be changed to accomodate that.
                    if(row.Field<int>("job_type") == 0)
                    {
                        string van = null;
                        if (row.Field<int>("van_type") == 0) { van = "Dry"; }
                        else { van = "Reefer"; }

                        addContract += " | FTL " + " | Van Type: " + van + " | Origin: " + row.Field<string>("origin_city") + " | Destination: " + row.Field<string>("destination_city");
                    }
                    else
                    {
                        string van = null;
                        if (row.Field<int>("van_type") == 0) { van = "Dry"; }
                        else { van = "Reefer"; }

                        addContract += " | LTL | Quantity: " + row.Field<int>("quantity") + " | Van Type: " + van + " | Origin: " + row.Field<string>("origin_city") + " | Destination: " + row.Field<string>("destination_city");

                    }


                    //Checks if this item has already been added to the list.
                    if (!NewOrdersListBox.Items.Contains(addContract))
                    {
                       NewOrdersListBox.Items.Add(addContract);
                    }

                }

                data.CloseConnection();

            }
            catch(Exception) 
            {
                BuyerLogger.warnBuyer("Unable to get new conrtacts.");
            }



        }

        private void NewOrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Updates the city textbox to be the origin city and stores the contract ID. Need to check if not null because of weird interaction when removing list item.
            if (NewOrdersListBox.SelectedItem != null)
            {
                String[] splitter = NewOrdersListBox.SelectedItem.ToString().Split('|');
                string city = splitter[splitter.Length - 2];
                string ID = splitter[0];


                splitter = ID.Split(' ');
                contractID = Convert.ToUInt32(splitter[1]);

                splitter = city.Split(' ');
                city = splitter[2];

                cityTxtbox.Text = city;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CompletedOrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Stores the order ID. Need to check if not null because of weird interaction when removing list item.
            if (CompletedOrdersListBox.SelectedItem != null)
            {
                String[] splitter = CompletedOrdersListBox.SelectedItem.ToString().Split('|');
                string city = splitter[splitter.Length - 2];
                string ID = splitter[0];


                splitter = ID.Split(' ');
                orderID = Convert.ToUInt32(splitter[1]);
            }

        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            DataTable orderTable = new DataTable();
            TMS_Data data = new TMS_Data();

            string query = "select * from orders where completed = \'true\'";



            //Checks the order table for completed orders when the button is clicked.
            try
            {
                data.OpenConnection();

                orderTable = data.SelectTMS(query);

                if (orderTable.Rows.Count > 0)
                {
                    foreach (DataRow row in orderTable.Rows)
                    {
                        string order = "ID: " + row.Field<uint>("order_ID") + " | Reefer: " + row.Field<int>("cargo_type") + " | Origin: " + row.Field<string>("origin_city") + " | Duration: " + row.Field<double>("trip_duration") + "h";
                        CompletedOrdersListBox.Items.Add(order);
                    }
                }

                data.CloseConnection();
            }
            catch (Exception)
            {
                BuyerLogger.warnBuyer("Unable to refresh the completed orders list.");
            }
        }
    }
}
