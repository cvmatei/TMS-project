using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
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

namespace TMS_Project
{
    /// <summary>
    /// Interaction logic for PlannerWindow.xaml
    /// </summary>
    public partial class PlannerWindow : Window
    {
        int order_id;
        int carrier_id;
        int weeks = 0;
        int sim = 0;
        string[] data;
        public PlannerWindow()
        {
            InitializeComponent();
            Planner plan = new Planner();
            data = plan.receiveOrder();
            
            foreach(string s in data)
            {
                if(s != null)
                {
                    OrdersToProcessListBox.Items.Add(s);
                }
            }

            if (OrdersToProcessListBox.SelectedItem != null)
            {
                string city;
                string[] carriers;
                string[] splitter = OrdersToProcessListBox.Items.CurrentItem.ToString().Split(' ');
                city = splitter[3];

                carriers = plan.showCarriers(city);

                foreach (string s in carriers)
                {
                    if(s != null)
                    {
                        AddTripComboBox.Items.Add(s);
                    }
                }
            }

            data = plan.activeOrder();

            foreach (string s in data)
            {
                if(s != null)
                {
                    ActiveOrdersListBox.Items.Add(s);
                }
            }

            data = plan.completeOrder();

            foreach (string s in data)
            {
                if(s != null)
                {
                    CompletedOrdersListBox.Items.Add(s);
                }
            }
        }

        private void Homebutton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }

        private void AddTripToOrderButton(object sender, RoutedEventArgs e)
        {
            if (AddTripComboBox.SelectedItem != null)
            {
                Planner plan = new Planner();
                string id;
                string[] splitter = AddTripComboBox.SelectedItem.ToString().Split(' ');
                id = splitter[1];
                carrier_id = Convert.ToInt32(id);
                plan.selectCarrier(order_id, carrier_id);

                PlannerWindow planWindow = new PlannerWindow();
                planWindow.Show();
                this.Close();
            }
        }

        private void CompleteOrderButton(object sender, RoutedEventArgs e)
        {
            if (ActiveOrdersListBox.SelectedItem != null)
            {
                Planner plan = new Planner();
                plan.confirmOrder(order_id);

                PlannerWindow planWindow = new PlannerWindow();
                planWindow.Show();
                this.Close();
            }
        }

        private void SimulateOneDayButton(object sender, RoutedEventArgs e)
        {
            if (ActiveOrdersListBox.SelectedItem != null)
            {
                int time;
                string item;
                Planner plan = new Planner();
                if (sim == 0)
                {
                    time = plan.simulateTimePassage(order_id);
                    if (time == 1)
                    {
                        item = ActiveOrdersListBox.SelectedItem.ToString();
                        ActiveOrdersListBox.Items.Remove(ActiveOrdersListBox.SelectedItem);
                        CompletedOrdersListBox.Items.Add(item);
                    }
                    else
                    {
                        sim = 1;
                    }
                }
                else
                {
                    item = ActiveOrdersListBox.SelectedItem.ToString();
                    ActiveOrdersListBox.Items.Remove(ActiveOrdersListBox.SelectedItem);
                    CompletedOrdersListBox.Items.Add(item);
                    sim = 0;
                }
            }
        }

        private void GenerateButton(object sender, RoutedEventArgs e)
        {
            string msg;
            Planner plan = new Planner();
            msg = plan.generateSummary(weeks);
            MessageBox.Show(msg);
        }

        private void AllTime_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            weeks = 0;
        }

        private void LastTwoWeeks_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            weeks = 1;
        }

        private void OrdersToProcessListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Planner planner = new Planner();
            if (OrdersToProcessListBox.SelectedItem != null)
            {
                string city;
                string[] carriers;
                string[] splitter = OrdersToProcessListBox.SelectedItem.ToString().Split(' ');
                city = splitter[4];
                order_id = Convert.ToInt32(splitter[1]);

                carriers = planner.showCarriers(city);

                AddTripComboBox.Items.Clear();

                foreach(string s in carriers)
                {
                    if(s != null)
                    {
                        AddTripComboBox.Items.Add(s);
                    }
                }
            }
        }

        private void ActiveOrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActiveOrdersListBox.SelectedItem != null)
            {
                string[] splitter = ActiveOrdersListBox.SelectedItem.ToString().Split(' ');
                order_id = Convert.ToInt32(splitter[1]);
            }
        }

        private void AddTripComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
