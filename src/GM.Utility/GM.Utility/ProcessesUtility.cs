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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for processes.
	/// </summary>
	[Obsolete("This class is obsolete. Get GM.Windows.Utility package with NuGet and use class ProcessUtility there.", true)]
	public static class ProcessesUtility
	{
		// FIXME obsolete 2018-01-31

		/// <summary>
		/// Gets the executable path of the specified process. Can be slow (~2 seconds) if the current process is 32bit and the specified process is 64bit.
		/// </summary>
		/// <param name="process">The process.</param>
		public static string GetProcessPath(Process process)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the executable path of the process with the specified ID. This can be quite slow (~2 seconds).
		/// </summary>
		/// <param name="processId">The unique identifier of the process.</param>
		public static string GetProcessPath(int processId)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether a process with the specified name is running on the local computer. Returns true if at least one instance is running.
		/// </summary>
		/// <param name="processName">The friendly name of the process.</param>
		public static bool IsRunning(string processName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Attempts to kill all non-exited processes in the provided list of processes. It also disposes all of them.
		/// </summary>
		/// <param name="processes">The processes to kill and dispose.</param>
		public static void KillAllProcesses(IEnumerable<Process> processes)
		{
			throw new NotImplementedException();
		}
	}
}
