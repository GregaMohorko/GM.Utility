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
		/// Gets only the decimal part of this decimal.
		/// <para>If the value is negative, the returned value will also be negative.</para>
		/// </summary>
		/// <param name="value">The decimal value.</param>
		public static decimal GetDecimals(this decimal value)
		{
			return value - decimal.Truncate(value);
		}

		/// <summary>
		/// Gets the specified number of decimals from the decimal part of this decimal number as an integer.
		/// <para>If the value is negative, the returned value will also be negative.</para>
		/// </summary>
		/// <param name="value">The decimal value.</param>
		/// <param name="decimalCount">Number of decimals to get.</param>
		public static int GetDecimalPart(this decimal value, int decimalCount)
		{
			decimal decimals = GetDecimals(value);
			return (int)(decimals * (int)(Math.Pow(10, decimalCount)));
		}

		/// <summary>
		/// Gets the whole decimal part of this decimal number as an integer.
		/// <para>If the value is negative, the returned value will also be negative.</para>
		/// </summary>
		/// <param name="value">The decimal value.</param>
		public static int GetDecimalPart(this decimal value)
		{
			decimal decimals = GetDecimals(value);
			int decimalPart = 0;
			int add;
			while(decimals != 0) {
				decimals *= 10;
				decimalPart *= 10;
				add = (int)decimals;
				decimalPart += add;
				decimals -= add;
			}
			return decimalPart;
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
