using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class DecimalExtensions
	{
		/// <summary>
		/// Gets the whole part of this decimal number.
		/// </summary>
		public static int GetWholePart(this decimal value)
		{
			return (int)decimal.Truncate(value);
		}

		/// <summary>
		/// Gets the decimal part of this decimal number as an integer.
		/// </summary>
		public static int GetDecimalPart(this decimal value)
		{
			decimal decimalPart = value - decimal.Truncate(value);
			return (int)(decimalPart * 100);
		}
	}
}
