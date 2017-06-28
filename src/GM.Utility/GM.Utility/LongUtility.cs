using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class LongUtility
	{
		/// <summary>
		/// Determines whether the provided string is a valid long.
		/// </summary>
		public static bool IsLong(string text)
		{
			long longg;
			if(long.TryParse(text, out longg))
				return true;

			return false;
		}

		/// <summary>
		/// Determines whether all the strings in the provided collection are a valid long.
		/// </summary>
		public static bool AreLongs(IEnumerable<string> texts)
		{
			return texts.All(s => IsLong(s));
		}

		/// <summary>
		/// Converts a collection of string representations of numbers to its 64-bit signed integer equivalents.
		/// </summary>
		public static IEnumerable<long> Parse(IEnumerable<string> texts)
		{
			return texts.Select(s => long.Parse(s));
		}

		/// <summary>
		/// Returns null if the provided text is not a valid integer.
		/// </summary>
		public static long? ParseNullable(string text)
		{
			long value;
			if(long.TryParse(text, out value))
				return value;

			return null;
		}

		/// <summary>
		/// Returns zero if the provided text is empty.
		/// </summary>
		public static long Parse0(string text)
		{
			if(text == string.Empty)
				return 0;

			return long.Parse(text);
		}
	}
}
