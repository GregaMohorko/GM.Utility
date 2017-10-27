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
			return date.Date.Subtract(TimeSpan.FromDays(date.Day - 1));
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
