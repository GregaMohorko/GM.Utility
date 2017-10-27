using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for SecureString.
	/// </summary>
	public static class SecureStringUtility
	{
		/// <summary>
		/// Converts input characters into a read-only secure string.
		/// </summary>
		/// <param name="input">The characters to convert.</param>
		[SecurityCritical]
		public static SecureString ToSecureString(this IEnumerable<char> input)
		{
			SecureString secure = new SecureString();

			foreach(char c in input) {
				secure.AppendChar(c);
			}

			secure.MakeReadOnly();

			return secure;
		}

		/// <summary>
		/// Converts a secure string to string.
		/// </summary>
		/// <param name="input">The secure string to convert.</param>
		[SecurityCritical]
		public static string ToInsecureString(this SecureString input)
		{
			IntPtr ptr = Marshal.SecureStringToBSTR(input);

			try {
				return Marshal.PtrToStringBSTR(ptr);
			} finally {
				Marshal.ZeroFreeBSTR(ptr);
			}
		}
	}
}
