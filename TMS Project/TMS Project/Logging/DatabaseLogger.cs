using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_Project.Logging
{
	/// <summary>
	/// Called DatabaseLogger.debugDatabase(string message)
	/// Logger for Database related logs. Calls functions from
	/// general TMSLogger class to perform funcions. 
	/// </summary>
	internal class DatabaseLogger
	{
		const string LOG_TYPE = "database";
		public static void infoDatabase(string message)
		{
			TMSLogger.WriteLog("Info", LOG_TYPE, message);
		}
		public static void debugDatabase(string message)
		{
			TMSLogger.WriteLog("Debug", LOG_TYPE, message);
		}
		public static void warnDatabase(string message)
		{
			TMSLogger.WriteLog("Warn", LOG_TYPE, message);
		}
		public static void fatalDatabase(string message)
		{
			TMSLogger.WriteLog("Fatal", LOG_TYPE, message);
		}
	}
}
