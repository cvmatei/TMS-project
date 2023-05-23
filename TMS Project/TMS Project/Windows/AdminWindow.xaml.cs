using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
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

        private void LogchangeButton(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeLogDirectoryWindow win = new ChangeLogDirectoryWindow();
                win.ShowDialog();
                if (!win.isExit)
                {
                    if (Admin.UpdateLogDirectory(win.LogType, win.ChangeLogDirectory)) {
				        MessageBox.Show(win.LogType + " log directory changed to " + win.ChangeLogDirectory);
			        }
                    else
                    {
                        MessageBox.Show("Could not change log directory");
                    }
                }
                win.Close();
            }
            catch (Exception ex)
            {
				AdminLogger.warnAdmin("Failed attempt to change log directory - " + ex.Message);
			}
            
		}

        private void OpenLogButton(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenLogWindow win = new OpenLogWindow();
                win.ShowDialog();
                if (!win.isExit)
                {
                    string logType = win.LogType;
				    string path = ConfigurationManager.AppSettings[logType];
				    string messageBoxText = TMSLogger.DumpLog(path);
                    if (!String.IsNullOrEmpty(messageBoxText))
                    {
                        MessageBox.Show(messageBoxText);
                    }
                    else
                    {
                        MessageBox.Show("Unable to read from file.", "Log File Read Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                win.Close();
            }
            catch (Exception ex)
            {
                AdminLogger.warnAdmin(ex.Message);
            }
        }

        private void IPChangeButton_Click(object sender, RoutedEventArgs e)
        {
            TMS_Data data = new TMS_Data();
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings["ipAddress"].Value = TargetIPTextBox.Text;
			config.Save();
           
			ConfigurationManager.RefreshSection("appSettings"); 
            
            data.UpdateConnectionString();

        }

        private void TargetPortChangeButton(object sender, RoutedEventArgs e)
        {
			TMS_Data data = new TMS_Data();
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings["ipAddress"].Value = TargetPortTextBox.Text;
			config.Save();

			ConfigurationManager.RefreshSection("appSettings");

			data.UpdateConnectionString();
		}

        private void EditRateTableButton(object sender, RoutedEventArgs e)
        {
            UpdateRateTable win = new UpdateRateTable();
            win.ShowDialog();
            if (!win.isExit)
            {
                Admin.UpdateCarrierRates(win.CarrierName, win.RateType, Convert.ToDouble(win.NewRate));
            }
            
            win.Close();
		}

		private void EditRouteTableButton(object sender, RoutedEventArgs e)
        {
			string queryString = DBQueryTextBox.Text.ToLower();
			try
			{
				if (!String.IsNullOrEmpty(queryString) && queryString.Contains("from trips"))
				{
					if (queryString.Split(' ')[0].Contains("update"))
					{
						TMS_Data conn = new TMS_Data();
						conn.OpenConnection();
						conn.UpdateTMS(queryString);
						conn.CloseConnection();
					}
					else if (queryString.Split(' ')[0].Contains("insert"))
					{
						TMS_Data conn = new TMS_Data();
						conn.OpenConnection();
						conn.InsertTMS(queryString);
						conn.CloseConnection();
					}
					else if (queryString.Split(' ')[0].Contains("delete"))
					{
						TMS_Data conn = new TMS_Data();
						conn.OpenConnection();
						conn.DeleteTMS(queryString);
						conn.CloseConnection();
					}
				}
				else
				{
					MessageBox.Show("Field cannot be empty and must reference the trips table.");
				}
			}
			catch (Exception ex)
			{
				AdminLogger.warnAdmin("Could not complete query on carriers table - " + ex.Message);
			}
		}

        private void EditCarrierInformationButton(object sender, RoutedEventArgs e)
        {
            string queryString = DBQueryTextBox.Text.ToLower();
            try
            {
                if (!String.IsNullOrEmpty(queryString) && queryString.Contains("from carriers"))
                {
                    if (queryString.Split(' ')[0].Contains("update"))
                    {
                        TMS_Data conn = new TMS_Data();
                        conn.OpenConnection();
                        conn.UpdateTMS(queryString);
                        conn.CloseConnection();
                    }
                    else if (queryString.Split(' ')[0].Contains("insert"))
                    {
				        TMS_Data conn = new TMS_Data();
				        conn.OpenConnection();
				        conn.InsertTMS(queryString);
				        conn.CloseConnection();
			        }
                    else if (queryString.Split(' ')[0].Contains("delete"))
                    {
				        TMS_Data conn = new TMS_Data();
				        conn.OpenConnection();
				        conn.DeleteTMS(queryString);
				        conn.CloseConnection();
			        }
                }
                else
                {
                    MessageBox.Show("Field cannot be empty and must reference the carriers table.");
                }
            }
            catch (Exception ex)
            {
                AdminLogger.warnAdmin("Could not complete query on carriers table - " + ex.Message);
            }

		}

        private void BackupDatabaseButton(object sender, RoutedEventArgs e)
        {
			string caption = "Backup Status";

			if (Admin.CreateBackUp())
            {
                string messageBoxText = "Database backup was successful.";
                MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Information);   
            }
            else
            {
				string messageBoxText = "Database backup was unsuccessful, try again.";
				MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
			}
            
        }
	}
}
