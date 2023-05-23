using System;
using System.Configuration;
using System.IO;
using System.Xml;
using TMS_Project.Logging;

namespace TMS_Project
{
	internal class TMSLogger
	{
		/// <summary>
		/// Called to initiate the writing of a log message to the currently active log file.
		/// The path for the log file is stored in App.config and is defaulted to the directory
		/// of the .exe for this application. 
		/// </summary>
		/// <param name="message">Log message to add after the datetime of the log being made</param>
		public static void WriteLog(string logLevel, string logType, string message)
		{
			try
			{
				// grab the path stored in the config file for each log file
				string path = ConfigurationManager.AppSettings[logType];

				StreamWriter sw;
				if (!File.Exists(path))
				{
					sw = File.CreateText(path);
				}
				else
				{
					sw = File.AppendText(path);
				}
				// log level, log type, message
				message = String.Format("[{0}] [{1}] - {2}",  
					logType,
					logLevel, 
					message);

				// call function to write into log file
				WriteLog(message, sw);

				sw.Flush();
				sw.Close();
			}
			catch (Exception e)
			{
				AdminLogger.warnAdmin("Could not write to file " + logType + " - " + e.Message);
			}
		}

		/// <summary>
		/// Writes the passed message to the log file, parsed with the current Datetime
		/// as an identifier
		/// </summary>
		/// <param name="message"></param>
		/// <param name="sw"></param>
		public static void WriteLog(string message, StreamWriter sw)
		{
			sw.WriteLine("{0} {1}", DateTime.Now.ToString("R"), message);
		}

		/// <summary>
		/// Read from pass file path and print line by line to the console
		/// </summary>
		/// <param name="path"></param>
		public static string DumpLog(string path)
		{
			if (File.Exists(path))
			{
				string fileText = File.ReadAllText(path);
				return fileText;
			}
			else
			{
				return String.Empty;
			}
		}

		/// <summary>
		/// Allows the user to update/change the path stored in App.config for the log file
		/// New path must be confirmed as valid path. If the path does not exist it will be created
		/// and the log file destination will be updated
		/// </summary>
		/// <param name="path">New path for log file</param>
		public static void SelectLogDirectory(string logType, string path)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			foreach (XmlElement element in xmlDoc.DocumentElement)
			{
				if (element.Name.Equals("appSettings"))
				{
					foreach (XmlNode node in element.ChildNodes)
					{
						if (node.Attributes[0].Value.Equals(logType))
						{
							node.Attributes[1].Value = path;
						}
					}
				}
			}
			xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			ConfigurationManager.RefreshSection("appSettings");

		}
	}
}
