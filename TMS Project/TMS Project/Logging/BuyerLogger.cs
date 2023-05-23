using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_Project.Logging
{
	/// <summary>
	/// Called BuyerLogger.debugBuyer(string message)
	/// Logger for Buyer related logs. Calls functions from
	/// general TMSLogger class to perform funcions. 
	/// </summary>
	internal class BuyerLogger
	{
		const string LOG_TYPE = "buyer";
		public static void infoBuyer(string message)
		{
			TMSLogger.WriteLog("Info", LOG_TYPE, message);
		}
		public static void debugBuyer(string message)
		{
			TMSLogger.WriteLog("Debug", LOG_TYPE, message);
		}
		public static void warnBuyer(string message)
		{
			TMSLogger.WriteLog("Warn", LOG_TYPE, message);
		}
		public static void fatalBuyer(string message)
		{
			TMSLogger.WriteLog("Fatal", LOG_TYPE, message);
		}
	}
}
