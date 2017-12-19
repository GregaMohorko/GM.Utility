/*
MIT License

Copyright (c) 2017 Grega Mohorko

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Project: GM.Utility
Created: 2017-10-27
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for Environment. Includes operating system.
	/// </summary>
	public static class EnvironmentUtility
	{
		/// <summary>
		/// Gets the bitness of the current Windows operating system. Either 32 or 64.
		/// <para>
		/// This method is deprecated, please use <see cref="GetOperatingSystemBitness"/> instead.
		/// </para>
		/// </summary>
		[Obsolete("Method GetWindowsBit is deprecated, please use GetOperatingSystemBitness instead.",true)]
		public static int GetWindowsBit()
		{
			// FIXME obsolete 2017-10-27
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

		/// <summary>
		/// Gets the root directory information of the system drive. The system drive is considered the one on which the windows directory is located.
		/// </summary>
		public static string GetSystemDrive()
		{
			string windowsPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
			return Path.GetPathRoot(windowsPath);
		}
	}
}
