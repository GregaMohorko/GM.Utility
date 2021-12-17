/*
MIT License

Copyright (c) 2021 Gregor Mohorko

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
	/// Utilities for DateTime.
	/// </summary>
	public static class DateTimeUtility
	{
		/// <summary>
		/// Calculates and returns the duration between the two specified date times.
		/// <para>The order of both date times is not important.</para>
		/// </summary>
		/// <param name="dateTime1">The first date time. Can be before or after the second date time.</param>
		/// <param name="dateTime2">The second date time. Can be before or after the first date time.</param>
		public static TimeSpan DurationBetween(DateTime dateTime1, DateTime dateTime2)
		{
			return dateTime1 < dateTime2
				? dateTime2.Subtract(dateTime1)
				: dateTime1.Subtract(dateTime2);
		}

		/// <summary>
		/// Returns the end of the current month.
		/// </summary>
		public static DateTime EndOfMonth()
		{
			return EndOfMonth(DateTime.Today);
		}

		/// <summary>
		/// Returns the end of the month of the specified date.
		/// </summary>
		/// <param name="date">The source date.</param>
		public static DateTime EndOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
		}

		/// <summary>
		/// Returns the end of the current week (sunday).
		/// </summary>
		public static DateTime EndOfWeek()
		{
			return EndOfWeek(DateTime.Today);
		}

		/// <summary>
		/// Returns the end of the week (sunday) of the specified date.
		/// </summary>
		/// <param name="date">The source date.</param>
		public static DateTime EndOfWeek(this DateTime date)
		{
			if(date.DayOfWeek == DayOfWeek.Sunday) {
				return date.Date;
			}

			int diff = 7 - (int)date.DayOfWeek;

			return date.Date.AddDays(diff);
		}

		/// <summary>
		/// Gets the full name of the week day of this date based on the current UI culture.
		/// </summary>
		/// <param name="date">The date of which to get the week day name.</param>
		public static string GetWeekDayName(this DateTime date)
		{
			return GlobalizationUtility.GetWeekDayName(date);
		}

		/// <summary>
		/// Gets the full name of the month of this date based on the current UI culture.
		/// </summary>
		/// <param name="date">The date of which to get the month name.</param>
		public static string GetMonthName(this DateTime date)
		{
			return GlobalizationUtility.GetMonthName(date);
		}

		/// <summary>
		/// Gets the abbreviated name of the month of this date based on the current UI culture.
		/// </summary>
		/// <param name="date">The date of which to get the month name.</param>
		public static string GetMonthNameAbbreviated(DateTime date)
		{
			return GlobalizationUtility.GetMonthNameAbbreviated(date);
		}

		/// <summary>
		/// Determines whether this date is in the current month.
		/// <para>
		/// Ignores the year. 2000-01-01 and 2001-01-01 will return true.
		/// </para>
		/// </summary>
		public static bool IsCurrentMonth(this DateTime date)
		{
			return DateTime.Today.Month == date.Month;
		}

		/// <summary>
		/// Determines whether the provided text represents a valid <see cref="DateTime"/>.
		/// </summary>
		/// <param name="text">A string, possibly representing a valid date and time.</param>
		public static bool IsDateTime(string text)
		{
			return DateTime.TryParse(text, out _);
		}

		/// <summary>
		/// Determines whether this date is in the future. Ignores time.
		/// </summary>
		/// <param name="date">The source date.</param>
		public static bool IsFutureDate(this DateTime date)
		{
			return date.Date.CompareTo(DateTime.Today) > 0;
		}

		/// <summary>
		/// Returns the next date from today on the specified day of the week. If the specified day of the week is today, it returns the next week.
		/// </summary>
		/// <param name="dayOfWeek">The target day of the week.</param>
		public static DateTime NextOnDay(DayOfWeek dayOfWeek)
		{
			return NextOnDay(DateTime.Today, dayOfWeek);
		}

		/// <summary>
		/// Returns the next date from the specified date on the specified day of the week. If the specified day of the week is the same as the one of the specified date, this method returns the next week.
		/// </summary>
		/// <param name="date">The source date.</param>
		/// <param name="dayOfWeek">The target day of the week.</param>
		public static DateTime NextOnDay(this DateTime date,DayOfWeek dayOfWeek)
		{
			if(date.DayOfWeek == dayOfWeek) {
				return date.AddDays(7);
			}

			do {
				date = date.AddDays(1);
			} while(date.DayOfWeek != dayOfWeek);

			return date;
		}

		/// <summary>
		/// Returns the start of the current month.
		/// </summary>
		public static DateTime StartOfMonth()
		{
			return StartOfMonth(DateTime.Today);
		}

		/// <summary>
		/// Returns the start of the month of the specified date.
		/// </summary>
		/// <param name="date">The source date.</param>
		public static DateTime StartOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		/// <summary>
		/// Returns the start of the current week (monday).
		/// </summary>
		public static DateTime StartOfWeek()
		{
			return StartOfWeek(DateTime.Today);
		}

		/// <summary>
		/// Returns the start of the week (monday) of the specified date.
		/// </summary>
		/// <param name="date">The source date.</param>
		public static DateTime StartOfWeek(this DateTime date)
		{
			if(date.DayOfWeek == DayOfWeek.Monday) {
				return date.Date;
			}

			int currentDay = (date.DayOfWeek == DayOfWeek.Sunday) ? 7 : (int)date.DayOfWeek;

			// Monday = 1
			int diff = 1 - currentDay;

			return date.Date.AddDays(diff);
		}
	}
}
