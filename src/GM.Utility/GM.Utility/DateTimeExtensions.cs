using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class DateTimeExtensions
	{
		public static DateTime StartOfMonth(this DateTime date)
		{
			return date.Date.Subtract(TimeSpan.FromDays(date.Day - 1));
		}

		public static DateTime StartOfWeek(this DateTime date)
		{
			if(date.DayOfWeek == DayOfWeek.Monday) {
				return date.Date;
			}

			int currentDay = (int)date.DayOfWeek;

			if(currentDay == 0)
				currentDay = 7;

			int diff = DayOfWeek.Monday - date.DayOfWeek;

			return date.Date.AddDays(diff);
		}

		public static DateTime EndOfWeek(this DateTime date)
		{
			if(date.DayOfWeek == DayOfWeek.Sunday)
				return date.Date;

			int diff = 7 - (int)date.DayOfWeek;

			return date.Date.AddDays(diff);
		}

		/// <summary>
		/// Determines whether this date is in the future.
		/// </summary>
		public static bool IsFutureDate(this DateTime date)
		{
			return date.Date.CompareTo(DateTime.Today) > 0;
		}

		/// <summary>
		/// Determines whether this date is in the current month.
		/// </summary>
		public static bool IsCurrentMonth(this DateTime date)
		{
			DateTime today = DateTime.Now;
			return today.Month == date.Month;
		}
	}
}
