using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMS_Project.Logging;

namespace TMS_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AdminButton(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            this.Close();
            adminWindow.Show();
        }

        private void BuyerButton(object sender, RoutedEventArgs e)
        {
            //Buyer buyer = new Buyer();
            //buyer.getNewContracts();
            BuyerWindow buyerWindow = new BuyerWindow();
            this.Close();
            buyerWindow.Show();
        }

        private void PlannerButton(object sender, RoutedEventArgs e)
        {
            PlannerWindow plannerWindow = new PlannerWindow();
            this.Close();
            plannerWindow.Show();
        }

        private void HomeButton(object sender, RoutedEventArgs e)
        {
            
            //Kind of like a refresh button
            MainWindow newMainWindow = new MainWindow();
            this.Close();
            newMainWindow.Show();
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            
            this.Close();

        }
    }
}
