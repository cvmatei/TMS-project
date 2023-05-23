using MySql.Data.MySqlClient;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS_Project.Logging;

namespace TMS_Project
{
	public class Admin
	{
		const string DATABASE_PATH = "tms_schema.sql";

		/// <summary>
		/// Updates the specified FTL/LTL rate in the carriers table of the TMS database
		/// Called from the Admin window view with the parameters specified there.
		/// </summary>
		/// <param name="carrierName">Admin must select the carrier from a list of 
		///							  of carrier_names, which is passed into this function</param>
		/// <param name="rateType">Admin must select the type of rate (FTL/LTL) to be updated
		///						   in the database before calling this function</param>
		/// <param name="newRate"></param>
		/// <returns></returns>
		public static bool UpdateCarrierRates(string carrierName, string rateType, double newRate)
		{
			try
			{
				string query = "UPDATE carriers SET " + 
							rateType + " = " + newRate + 
							" WHERE carrier_name = " + carrierName + ";";

				// perform database functions 
				TMS_Data conn = new TMS_Data();
				conn.OpenConnection();
				conn.UpdateTMS(query);
				conn.CloseConnection();

				AdminLogger.infoAdmin(String.Format("Successful changes made to Carrier table: Carrier Name: {0} - Rate Type: {1} - New Rate - {2}", 
					carrierName, rateType, newRate));
				return true;
			}
			catch (Exception e)
			{
				AdminLogger.warnAdmin(e.Message);

				return false;
			}
		}
		public static bool UpdateLogDirectory (string logType, string newPath)
		{
			try
			{
				TMSLogger.SelectLogDirectory(logType, newPath);
				return true;
			}
			catch (Exception e)
			{
				AdminLogger.warnAdmin("Failed to change " + logType + "path - " + e.Message);
				return false; 
			}
		}
		/// <summary>
		/// Called to create a backup of the .sql file stored in the project directory
		/// If the file exists the contents will be overwritten with the new data
		/// </summary>
		/// <returns></returns>
		public static bool CreateBackUp()
		{
			try
			{
				string path = Path.Combine(Directory.GetParent(
					System.IO.Directory.GetCurrentDirectory()).
					Parent.Parent.Parent.FullName);

				string fileName = DateTime.Now.ToString("d") +
								"_" + DATABASE_PATH;

				string filePath = path + "\\Backup\\" + fileName;

				// perform database functions 
				TMS_Data conn = new TMS_Data();
				conn.OpenConnection();
				conn.BackUpDatabase(filePath);
				conn.CloseConnection();

				AdminLogger.infoAdmin("Database saved into backup file: " + fileName);
				return true;
			}
			catch (Exception e)
			{
				AdminLogger.warnAdmin(e.Message);

				return false;
			}
		}

		/// <summary>
		/// Called to create a backup of the .sql file stored in the project directory
		/// The backup data is imported into the server connection to restore the database
		/// back to the state saved in the backup file.
		/// @WorkOn - when choosing to restore from backup the Admin must select the .sql file
		/// to use as the backup state and pass it to this function.
		/// </summary>
		/// <returns></returns>
		public static bool RestoreFromBackUp(string fileName)
		{
			try
			{
				string path = Path.Combine(Directory.GetParent(
					System.IO.Directory.GetCurrentDirectory()).
					Parent.Parent.Parent.FullName);

				string filePath = path + "\\Backup\\" +
								fileName;

				// perform database functions 
				TMS_Data conn = new TMS_Data();
				conn.OpenConnection();
				conn.RestoreDatabase(filePath);
				conn.CloseConnection();

				AdminLogger.infoAdmin("Database restored from backup saved in file: " + fileName);

				return true;
			}
			catch (Exception e)
			{
				AdminLogger.fatalAdmin(e.Message);

				return false;
			}
		}

	}
}
