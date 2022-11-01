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
using System.Globalization;
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
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The decimal value.</param>
		public static decimal GetDecimals(this decimal value)
		{
			return value - decimal.Truncate(value);
		}

		/// <summary>
		/// Gets only the decimal part of this decimal, rounded to the specified number of decimals.
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The decimal value.</param>
		/// <param name="decimalCount">Number of decimals to round to.</param>
		public static decimal GetDecimals(this decimal value, int decimalCount)
		{
			value = Math.Round(value, decimalCount);
			return value - decimal.Truncate(value);
		}

		/// <summary>
		/// Gets the specified number of decimals from the decimal part of this decimal number as an integer.
		/// <para>If the decimal part is below 0.1, the zeros at the beginning will be omitted.</para>
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
		/// </summary>
		/// <param name="value">The decimal value.</param>
		/// <param name="decimalCount">Number of decimals to get.</param>
		/// <param name="round">Determines whether or not it should round the number in case the decimalCount parameter is lower than the decimals in the value.</param>
		public static int GetDecimalPart(this decimal value, int decimalCount, bool round)
		{
			decimal decimals = GetDecimals(value);
			if(round) {
				decimals = Math.Round(decimals, decimalCount);
			}
			return (int)(decimals * (int)Math.Pow(10, decimalCount));
		}

		/// <summary>
		/// Gets the whole decimal part of this decimal number as an integer.
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
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
		/// <para>If the value is negative, the returned value will also be negative (except if the returned value is zero, then the information about the sign is lost).</para>
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
			return decimal.TryParse(text, out _);
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

		/// <summary>
		/// Converts this decimal value to a string of fixed length.
		/// </summary>
		/// <param name="value">The decimal value to convert to string.</param>
		/// <param name="length">The length of the string to convert to.</param>
		/// <param name="cultureInfo">The culture to use. If not provided, the <see cref="CultureInfo.CurrentCulture"/> will be used.</param>
		public static string ToStringFixedLength(this decimal value, int length, CultureInfo cultureInfo = null)
		{
			if(length < 1) {
				throw new ArgumentOutOfRangeException(nameof(length), length, "The minimum length is 1.");
			}

			if(cultureInfo == null) {
				cultureInfo = CultureInfo.CurrentCulture;
			}

			var sb = new StringBuilder(length);
			// negative sign
			if(value < 0) {
				if(length < 1 + cultureInfo.NumberFormat.NegativeSign.Length) {
					throw new ArgumentOutOfRangeException(nameof(length), length, $"The minimum length for a negative value is 1 plus the length of the negative sign (negative sign used: '{cultureInfo.NumberFormat.NegativeSign}').");
				}
				sb.Append(cultureInfo.NumberFormat.NegativeSign);
			}
			// the whole part
			{
				int wholePart = Math.Abs(value.GetWholePart());
				if(wholePart > 0) {
					sb.Append(wholePart.ToString());
				}
			}

			if(sb.Length > length) {
				throw new ArgumentOutOfRangeException(nameof(length), length, $"The value '{value}' could not be converted to a string of length {length} because the mandatory part of the value (negative sign plus the integral part) is longer. Either increase the desired length or provide a lower value.");
			}

			// decimals
			if(sb.Length < length) {
				int charsLeft = length - sb.Length;
				// decimal separator
				if(cultureInfo.NumberFormat.NumberDecimalSeparator.Length >= charsLeft) {
					sb.Append(cultureInfo.NumberFormat.NumberDecimalSeparator.Substring(0, charsLeft));
				} else {
					sb.Append(cultureInfo.NumberFormat.NumberDecimalSeparator);
					charsLeft -= cultureInfo.NumberFormat.NumberDecimalSeparator.Length;
					decimal decimalPart = Math.Abs(value.GetDecimals(charsLeft));
					if(decimalPart == 0) {
						sb.Append('0', charsLeft);
					} else {
						string decimalPartString = decimalPart.ToString(cultureInfo).Substring(1 + cultureInfo.NumberFormat.NumberDecimalSeparator.Length);
						if(decimalPartString.Length > charsLeft) {
							decimalPartString = decimalPartString.Substring(0, charsLeft);
						}
						sb.Append(decimalPartString);
					}
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Converts this decimal value to a string of the shortest general format.
		/// <para>Uses the format: 0.##################...</para>
		/// </summary>
		/// <param name="value">The decimal value to convert to string.</param>
		/// <param name="cultureInfo">The culture to use. If not provided, the <see cref="CultureInfo.CurrentCulture"/> will be used.</param>
		public static string ToStringShortest(this decimal value, CultureInfo cultureInfo = null)
		{
			if(cultureInfo == null) {
				cultureInfo = CultureInfo.CurrentCulture;
			}

			const string FORMAT = "0.##################################################################";
			return value.ToString(FORMAT, cultureInfo);
		}
	}
}
