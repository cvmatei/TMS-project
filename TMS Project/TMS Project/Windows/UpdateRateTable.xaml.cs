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
	/// Interaction logic for UpdateRateTable.xaml
	/// </summary>
	public partial class UpdateRateTable : Window
	{
		public UpdateRateTable()
		{
			InitializeComponent();
		}
		public bool isExit = true; // flag indicating which button the user clicked
		public string CarrierName
		{
			get { return CarrierNameTextBox.Text; }
			set { CarrierNameTextBox.Text = value; }
		}
		public string NewRate
		{
			get { return NewRateTextBox.Text; }
			set { NewRateTextBox.Text = value; }
		}
		public string RateType
		{
			get { return RateTypeTextBox.Text; }
			set { RateTypeTextBox.Text = value; }
		}
		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			
			if (!String.IsNullOrEmpty(CarrierNameTextBox.Text) &&
				!String.IsNullOrEmpty(NewRateTextBox.Text) &&
				!String.IsNullOrEmpty(RateTypeTextBox.Text))
			{
				isExit = false;
				CarrierName = CarrierNameTextBox.Text;
				NewRate = NewRateTextBox.Text;
				RateType = RateTypeTextBox.Text;
				this.Hide();
			}
			else
			{
				MessageBox.Show("Must fill out fields or Click Exit Button");
			}
		}
	}
}
