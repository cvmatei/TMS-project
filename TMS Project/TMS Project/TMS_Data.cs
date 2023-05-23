using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Pkcs;
using TMS_Project.Logging;

namespace TMS_Project
{
	/// <summary>
	/// Standard Flow: Create TMS_Data object -> OpenConnection() ->
	///				   Perform Select/Insert/Update/Delete until connection
	///				   is no longer needed -> CloseConnection()
	/// </summary>
	public class TMS_Data
	{
		private readonly MySqlConnection connection;
		private string serverIP;
		private string serverPort;
		private readonly string database;
		private readonly string uid;
		private readonly string password;
		private string connectionString;

		/// <summary>
		/// TMS database connection object constructor
		/// </summary>
		public TMS_Data()
		{
			// variables required to connect to database
			serverIP = ConfigurationManager.AppSettings["ipAddress"];
			serverPort = ConfigurationManager.AppSettings["port"];
			database = "tmsdb";
			uid = "peter";
			password = "conestoga123";

			// parse variables together to create connection string
			UpdateConnectionString();
			connection = new MySqlConnection(connectionString);
		}

		public void UpdateConnectionString()
		{
			connectionString = "server=" + serverIP +
									";port=" + serverPort +
									";database=" + database +
									";uid=" + uid +
									";pwd=" + password + ";";
		}

		/// <summary>
		/// General function to open connection to MySQL object
		/// </summary>
		/// <returns></returns>
		public bool OpenConnection()
		{
			try
			{
				connection.Open();
				return true;
			}
			catch(MySqlException e)
			{
				switch (e.Number)
				{
					case 0:
						DatabaseLogger.fatalDatabase("Cannot connect to server");
						break;
					case 1:
						DatabaseLogger.fatalDatabase("Invalid uid / password");
						break;
				}
				return false;
			}
		}

		/// <summary>
		/// General function to close connection to MySQL object
		/// </summary>
		/// <returns></returns>
		public bool CloseConnection()
		{
			try
			{
				connection.Close();
				return true;
			}
			catch (MySqlException e)
			{
				DatabaseLogger.fatalDatabase(e.Message);
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public bool BackUpDatabase(string filePath)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = connection;
				MySqlBackup mb = new MySqlBackup(cmd);

				mb.ExportToFile(filePath);
				return true;
			}
			catch (Exception e)
			{
				DatabaseLogger.fatalDatabase(e.Message);
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public bool RestoreDatabase(string filePath)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = connection;
				MySqlBackup mb = new MySqlBackup(cmd);

				mb.ImportFromFile(filePath);

				return true;
			}
			catch (Exception e)
			{
				DatabaseLogger.fatalDatabase(e.Message);
				return false;
			}
		}

		/// <summary>
		/// Pass a SELECT query to be performed on the connection object,
		/// results are returned as a DataTable to have the information 
		/// parsed by the caller.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DataTable SelectTMS(string query)
		{
			var table = new DataTable();

			using (var da = new MySqlDataAdapter(query, connection))
			{
				da.Fill(table);
			}

			return table;
		}

		/// <summary>
		/// Pass an INSERT query to be performed on the connection object,
		/// returns "true" if successful, otherwise returns false and writes
		/// cause of error as "warn" in the databaseLog
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool InsertTMS(string query)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader reader = cmd.ExecuteReader();
				reader.Close();

				return true;
			}
			catch (Exception e)
			{
				DatabaseLogger.warnDatabase(e.Message);

				return false;
			}
		}

		/// <summary>
		/// Pass an UPDATE query to be performed on the connection object,
		/// returns "true" if successful, otherwise returns false and writes
		/// cause of error as "warn" in the databaseLog
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool UpdateTMS(string query)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader reader = cmd.ExecuteReader();
				

				return true;
			}
			catch (Exception e)
			{
				DatabaseLogger.warnDatabase(e.Message);

				return false;
			}
		}

		/// <summary>
		/// Pass an DELETE query to be performed on the connection object,
		/// returns "true" if successful, otherwise returns false and writes
		/// cause of error as "warn" in the databaseLog
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public bool DeleteTMS(string query)
		{
			try
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader reader = cmd.ExecuteReader();
				reader.Close();

				return true;
			}
			catch (Exception e)
			{
				DatabaseLogger.warnDatabase(e.Message);

				return false;
			}
		}
	}
}
