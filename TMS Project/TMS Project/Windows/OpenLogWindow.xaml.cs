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
	/// Interaction logic for OpenLogWindow.xaml
	/// </summary>
	public partial class OpenLogWindow : Window
	{
		public bool isExit = true;
		public OpenLogWindow()
		{
			InitializeComponent();
		}
		public string LogType
		{
			get { return logTypeComboBox.Text; }
			set { logTypeComboBox.Text = value; }	
		}

		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(logTypeComboBox.Text))
			{
				isExit = false;
				LogType = logTypeComboBox.Text.ToLower();
				this.Hide();
			}
			else
			{
				MessageBox.Show("Select log type or Click Exit Button");
			}
		}
	}
}
