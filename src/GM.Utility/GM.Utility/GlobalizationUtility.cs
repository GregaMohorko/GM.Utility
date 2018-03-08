/*
MIT License

Copyright (c) 2018 Grega Mohorko

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
Created: 2018-2-25
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for globalization. Uses current UI culture to determine the date format.
	/// </summary>
	public static class GlobalizationUtility
	{
		/// <summary>
		/// Gets the full name of the week day of the specified date based on the current UI culture.
		/// </summary>
		/// <param name="date">The date of which to get the week day name.</param>
		public static string GetWeekDayName(DateTime date)
		{
			return GetWeekDayName(date.DayOfWeek);
		}

		/// <summary>
		/// Gets the full name of the specified week day based on the current UI culture.
		/// </summary>
		/// <param name="dayOfWeek">The week day for which to get the name.</param>
		public static string GetWeekDayName(DayOfWeek dayOfWeek)
		{
			DateTimeFormatInfo dtfi = CultureInfo.CurrentUICulture.DateTimeFormat;
			return dtfi.GetDayName(dayOfWeek);
		}

		/// <summary>
		/// Gets the full name of the month of the specified date based on the current UI culture.
		/// </summary>
		/// <param name="date">The date of which to get the month name.</param>
		public static string GetMonthName(DateTime date)
		{
			return GetMonthName(date.Month);
		}

		/// <summary>
		/// Gets the full name of the specified month based on the current UI culture.
		/// </summary>
		/// <param name="month">The month for which to get the name.</param>
		public static string GetMonthName(int month)
		{
			DateTimeFormatInfo dtfi = CultureInfo.CurrentUICulture.DateTimeFormat;
			return dtfi.GetMonthName(month);
		}

		/// <summary>
		/// Gets the abbreviated name of the month of the specified date based on the current UI culture.
		/// </summary>
		/// <param name="date">The date of which to get the month name.</param>
		public static string GetMonthNameAbbreviated(DateTime date)
		{
			return GetMonthNameAbbreviated(date.Month);
		}

		/// <summary>
		/// Gets the abbreviated name of the specified month based on the current UI culture.
		/// </summary>
		/// <param name="month">The month for which to get the name.</param>
		public static string GetMonthNameAbbreviated(int month)
		{
			DateTimeFormatInfo dtfi = CultureInfo.CurrentUICulture.DateTimeFormat;
			return dtfi.GetAbbreviatedMonthName(month);
		}
	}
}
