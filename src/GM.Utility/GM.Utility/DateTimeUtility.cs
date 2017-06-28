using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class DateTimeUtility
	{
		/// <summary>
		/// Returns the next date on the provided day. If the provided day is today, it returns next week.
		/// </summary>
		public static DateTime NextOnDay(DayOfWeek dayOfWeek)
		{
			DateTime currentDate = DateTime.Today;

			if(currentDate.DayOfWeek == dayOfWeek)
				return currentDate.AddDays(7);

			do {
				currentDate = currentDate.AddDays(1);
			} while(currentDate.DayOfWeek != dayOfWeek);

			return currentDate;
		}
	}
}
