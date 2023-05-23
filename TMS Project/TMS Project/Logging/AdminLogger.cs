using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_Project.Logging
{
	/// <summary>
	/// Called AdminLogger.debugAdmin(string message)
	/// Logger for Admin related logs. Calls functions from
	/// general TMSLogger class to perform funcions. 
	/// </summary>
	internal class AdminLogger
	{
		const string LOG_TYPE = "admin";
		public static void infoAdmin(string message)
		{
			TMSLogger.WriteLog("Info", LOG_TYPE, message);
		}
		public static void debugAdmin(string message)
		{
			TMSLogger.WriteLog("Debug", LOG_TYPE, message);
		}
		public static void warnAdmin(string message)
		{
			TMSLogger.WriteLog("Warn", LOG_TYPE, message);
		}
		public static void fatalAdmin(string message)
		{
			TMSLogger.WriteLog("Fatal", LOG_TYPE, message);
		}
	}
}
