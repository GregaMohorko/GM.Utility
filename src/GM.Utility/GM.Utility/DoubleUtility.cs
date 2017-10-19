using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class DoubleUtility
	{
		/// <summary>
		/// Determines whether the provided string is a valid double.
		/// </summary>
		public static bool IsDouble(string text)
		{
			return double.TryParse(text, out double value);
		}

		/// <summary>
		/// Returns null if the provided text is not a valid double.
		/// </summary>
		public static double? ParseNullable(string text)
		{
			if(double.TryParse(text,out double value)) {
				return value;
			}
			return null;
		}

		/// <summary>
		/// Returns zero if the provided text is empty.
		/// </summary>
		public static double Parse0(string text)
		{
			if(text == string.Empty) {
				return 0;
			}
			return double.Parse(text);
		}
	}
}
