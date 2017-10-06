using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class EnvironmentUtility
	{
		/// <summary>
		/// Gets the bitness of the current Windows operating system. Either 32 or 64.
		/// </summary>
		public static int GetWindowsBit()
		{
			return Environment.Is64BitOperatingSystem ? 64 : 32;
		}
	}
}
