/*
MIT License

Copyright (c) 2018 Grega Mohorko

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
Created: 2018-12-24
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for network information.
	/// </summary>
	public static class NetworkUtility
	{
		/// <summary>
		/// Returns the MAC (Media Access Control or physical address) for the first currently active (can transmit data packets) network interface (adapter) on the local computer.
		/// <para>If you want a <see cref="PhysicalAddress"/>, call <see cref="GetPhysicalAddress"/>.</para>
		/// </summary>
		public static string GetMacAddress()
		{
			return GetPhysicalAddress()?.ToString();
		}

		/// <summary>
		/// Returns the physical address (or Media Access Control (MAC)) for the first currently active (can transmit data packets) network interface (adapter) on the local computer.
		/// <para>If you want a string, call <see cref="GetMacAddress"/>.</para>
		/// </summary>
		public static PhysicalAddress GetPhysicalAddress()
		{
			return GetNetworkInterface()?.GetPhysicalAddress();
		}

		/// <summary>
		/// Returns the first currently active (can transmit data packets) network interface (adapter) on the local computer.
		/// </summary>
		public static NetworkInterface GetNetworkInterface()
		{
			return GetNetworkInterfaces().FirstOrDefault();
		}

		/// <summary>
		/// Returns the currently active (can transmit data packets) network interfaces (adapters) on the local computer.
		/// </summary>
		public static IEnumerable<NetworkInterface> GetNetworkInterfaces()
		{
			return NetworkInterface.GetAllNetworkInterfaces()
				.Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);
		}
	}
}
