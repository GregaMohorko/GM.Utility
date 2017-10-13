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
		/// <para>
		/// This method is deprecated, please use GetOperatingSystemBitness instead.
		/// </para>
		/// </summary>
		[Obsolete("Method GetWindowsBit is deprecated, please use GetOperatingSystemBitness instead.",false)]
		public static int GetWindowsBit()
		{
			return GetOperatingSystemBitness();
		}
		
		/// <summary>
		/// Gets the bitness of the current operating system. Either 32 or 64.
		/// </summary>
		public static int GetOperatingSystemBitness()
		{
			return Environment.Is64BitOperatingSystem ? 64 : 32;
		}

		/// <summary>
		/// Gets the architecture of the current operating system. Either "x64" or "x86".
		/// </summary>
		public static string GetOperatingSystemArchitecture()
		{
			return Environment.Is64BitOperatingSystem ? "x64" : "x86";
		}

		/// <summary>
		/// Gets the bitness of the current process. Either 32 or 64.
		/// </summary>
		public static int GetProcessBitness()
		{
			return Environment.Is64BitProcess ? 64 : 32;
		}

		/// <summary>
		/// Gets the architecture of the current process. Either "x64" or "x86".
		/// </summary>
		public static string GetProcessArchitecture()
		{
			return Environment.Is64BitProcess ? "x64" : "x86";
		}
	}
}
