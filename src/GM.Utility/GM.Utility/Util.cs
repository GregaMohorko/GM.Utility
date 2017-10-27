using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// General utilities.
	/// </summary>
	public static class Util
	{
		/// <summary>
		/// Swaps the values in both provided variables.
		/// </summary>
		/// <typeparam name="T">The type of the variables.</typeparam>
		/// <param name="A">First variable.</param>
		/// <param name="B">Second variable.</param>
		public static void Swap<T>(ref T A,ref T B)
		{
			T swap = A;
			A = B;
			B = swap;
		}
	}
}
