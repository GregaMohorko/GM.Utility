using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class ProcessesUtility
	{
		/// <summary>
		/// Determines whether a process with the specified name is running on the local computer. Returns true even if more than one instance is running.
		/// </summary>
		/// <param name="processName">The friendly name of the process.</param>
		public static bool IsRunning(string processName)
		{
			Process[] processes = Process.GetProcessesByName(processName);
			return processes.Length > 0;
		}
	}
}
