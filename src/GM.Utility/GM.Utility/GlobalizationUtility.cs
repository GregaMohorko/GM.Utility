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
