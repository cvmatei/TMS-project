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
using System.Windows.Shapes;

namespace TMS_Project
{
	/// <summary>
	/// Interaction logic for UpdateCarrierWindow.xaml
	/// </summary>
	public partial class UpdateCarrierWindow : Window
	{
		public UpdateCarrierWindow()
		{
			InitializeComponent();
		}
		public bool Canceled = true;
		public string CarrierName
		{
			get { return CarrierNameTextBox.Text; }
			set { CarrierNameTextBox.Text = value; }
		}
		public string QueryString
		{
			get { return CarrierQueryTextBox.Text; }
			set { CarrierQueryTextBox.Text = value; }
		}

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(CarrierNameTextBox.Text) && 
				!String.IsNullOrEmpty(CarrierQueryTextBox.Text))
			{
				CarrierName = CarrierNameTextBox.Text;
				QueryString = CarrierQueryTextBox.Text;
				this.Close();
			}
			else
			{
				MessageBox.Show("Must provide fill out fields or Click Exit Button");
			}
		}
	}
}
