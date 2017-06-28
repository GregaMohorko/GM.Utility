using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class DecimalUtility
	{
		/// <summary>
		/// Determines whether the provided string is a valid decimal.
		/// </summary>
		public static bool IsDecimal(string text)
		{
			decimal value;
			if(decimal.TryParse(text, out value)) {
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns null if the provided text is not a valid decimal.
		/// </summary>
		public static decimal? ParseNullable(string text)
		{
			decimal value;
			if(decimal.TryParse(text, out value))
				return value;

			return null;
		}

		/// <summary>
		/// Returns zero if the provided text is empty.
		/// </summary>
		public static decimal Parse0(string text)
		{
			if(text == string.Empty)
				return 0;

			return decimal.Parse(text);
		}
	}
}
