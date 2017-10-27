using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for decimal.
	/// </summary>
	public static class DecimalUtility
	{
		/// <summary>
		/// Determines whether all the texts in the provided collection represent a valid decimal.
		/// </summary>
		/// <param name="texts">A collection of texts.</param>
		public static bool AreDecimals(IEnumerable<string> texts)
		{
			return texts.All(t => IsDecimal(t));
		}
		
		/// <summary>
		/// Gets the decimal part of this decimal number.
		/// </summary>
		/// <param name="value">The decimal value.</param>
		public static int GetDecimalPart(this decimal value)
		{
			decimal decimalPart = value - decimal.Truncate(value);
			return (int)(decimalPart * 100);
		}

		/// <summary>
		/// Gets the whole part of this decimal number.
		/// </summary>
		/// <param name="value">The decimal value.</param>
		public static int GetWholePart(this decimal value)
		{
			return (int)decimal.Truncate(value);
		}

		/// <summary>
		/// Determines whether the provided text represents a valid decimal.
		/// </summary>
		/// <param name="text">The text.</param>
		public static bool IsDecimal(string text)
		{
			return decimal.TryParse(text, out decimal value);
		}

		/// <summary>
		/// Converts a collection of string representations of decimals to actual decimals.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<decimal> Parse(IEnumerable<string> texts)
		{
			return texts.Select(t => decimal.Parse(t));
		}

		/// <summary>
		/// Converts a collection of string representations of decimals to actual decimals. Sets zero if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<decimal> Parse0(IEnumerable<string> texts)
		{
			return texts.Select(t => Parse0(t));
		}

		/// <summary>
		/// Parses the provided text to a decimal. Returns zero if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static decimal Parse0(string text)
		{
			return ParseNullable(text) ?? 0;
		}

		/// <summary>
		/// Converts a collection of string representations of decimals to actual decimals. Sets null if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<decimal?> ParseNullable(IEnumerable<string> texts)
		{
			return texts.Select(t => ParseNullable(t));
		}

		/// <summary>
		/// Parses the provided text to a decimal. Returns null if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static decimal? ParseNullable(string text)
		{
			if(decimal.TryParse(text, out decimal value)) {
				return value;
			}
			return null;
		}
	}
}
