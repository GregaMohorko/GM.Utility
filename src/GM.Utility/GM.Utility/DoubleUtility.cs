/*
MIT License

Copyright (c) 2020 Gregor Mohorko

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
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for double.
	/// </summary>
	public static class DoubleUtility
	{
		/// <summary>
		/// Determines whether all the texts in the provided collection represent a valid double.
		/// </summary>
		/// <param name="texts">A collection of texts.</param>
		public static bool AreDoubles(IEnumerable<string> texts)
		{
			return texts.All(t => IsDouble(t));
		}

		/// <summary>
		/// Gets only the decimal part of this double.
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The double value.</param>
		public static double GetDecimals(this double value)
		{
			decimal decimalValue = Convert.ToDecimal(value);
			decimal decimals = decimalValue.GetDecimals();
			return decimal.ToDouble(decimals);
		}

		/// <summary>
		/// Gets only the decimal part of this double, rounded to the specified number of decimals.
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The double value.</param>
		/// <param name="decimalCount">Number of decimals to round to.</param>
		public static double GetDecimals(this double value, int decimalCount)
		{
			decimal decimalValue = Convert.ToDecimal(value);
			decimal decimals = decimalValue.GetDecimals(decimalCount);
			return decimal.ToDouble(decimals);
		}

		/// <summary>
		/// Gets the specified number of decimals from the decimal part of this double as an integer.
		/// <para>If the decimal part is below 0.1, the zeros at the beginning will be omitted.</para>
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The double value.</param>
		/// <param name="decimalCount">Number of decimals to get.</param>
		/// <param name="round">Determines whether or not it should round the number in case the decimalCount parameter is lower than the decimals in the value.</param>
		public static int GetDecimalPart(this double value, int decimalCount, bool round)
		{
			decimal decimalValue = Convert.ToDecimal(value);
			return decimalValue.GetDecimalPart(decimalCount, round);
		}

		/// <summary>
		/// Gets the whole decimal part of this double as an integer.
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The double value.</param>
		public static int GetDecimalPart(this double value)
		{
			decimal decimalValue = Convert.ToDecimal(value);
			return decimalValue.GetDecimalPart();
		}

		/// <summary>
		/// Determines whether the provided text represents a valid double.
		/// </summary>
		/// <param name="text">The text.</param>
		public static bool IsDouble(string text)
		{
			return double.TryParse(text, out double value);
		}

		/// <summary>
		/// Converts a collection of string representations of doubles to actual doubles.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<double> Parse(IEnumerable<string> texts)
		{
			return texts.Select(t => double.Parse(t));
		}

		/// <summary>
		/// Converts a collection of string representations of doubles to actual doubles. Sets zero if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<double> Parse0(IEnumerable<string> texts)
		{
			return texts.Select(t => Parse0(t));
		}

		/// <summary>
		/// Parses the provided text to a double. Returns zero if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static double Parse0(string text)
		{
			return ParseNullable(text) ?? 0;
		}

		/// <summary>
		/// Converts a collection of string representations of doubles to actual doubles. Sets null if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<double?> ParseNullable(IEnumerable<string> texts)
		{
			return texts.Select(t => ParseNullable(t));
		}

		/// <summary>
		/// Parses the provided text to a double. Returns null if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static double? ParseNullable(string text)
		{
			if(double.TryParse(text, out double value)) {
				return value;
			}
			return null;
		}
	}
}
