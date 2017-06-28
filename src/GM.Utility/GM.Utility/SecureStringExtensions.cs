using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class SecureStringExtensions
	{
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
