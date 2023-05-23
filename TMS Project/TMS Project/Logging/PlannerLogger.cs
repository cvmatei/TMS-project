using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS_Project.Logging
{
	/// <summary>
	/// Called PlannerLogger.debugPlanner(string message)
	/// Logger for Planner related logs. Calls functions from
	/// general TMSLogger class to perform funcions. 
	/// </summary>
	internal class PlannerLogger
	{
		const string LOG_TYPE = "planner";
		public static void infoPlanner(string message)
		{
			TMSLogger.WriteLog("Info", LOG_TYPE, message);
		}
		public static void debugPlanner(string message)
		{
			TMSLogger.WriteLog("Debug", LOG_TYPE, message);
		}
		public static void warnPlanner(string message)
		{
			TMSLogger.WriteLog("Warn", LOG_TYPE, message);
		}
		public static void fatalPlanner(string message)
		{
			TMSLogger.WriteLog("Fatal", LOG_TYPE, message);
		}
	}
}
