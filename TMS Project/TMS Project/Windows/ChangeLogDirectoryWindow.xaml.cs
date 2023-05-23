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
	/// Interaction logic for ChangeLogDirectoryWindow.xaml
	/// </summary>
	public partial class ChangeLogDirectoryWindow : Window
	{
		public bool isExit = true;	
		public ChangeLogDirectoryWindow()
		{
			InitializeComponent();
		}

		public string ChangeLogDirectory
		{
			get { return LogPathTextBox.Text; }
			set { LogPathTextBox.Text = value; }
		}
		public string LogType
		{
			get { return logTypeComboBox.Text; }
			set { logTypeComboBox.Text = value; }
		}
		private void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(LogPathTextBox.Text) &&
				!String.IsNullOrEmpty(logTypeComboBox.Text))
			{
				isExit = false;
				LogType = logTypeComboBox.Text.ToLower();
				ChangeLogDirectory = LogPathTextBox.Text;
				this.Hide();
			}
			else
			{
				MessageBox.Show("Must fill out fields or Click Exit Button");
			}
		}
	}
}
