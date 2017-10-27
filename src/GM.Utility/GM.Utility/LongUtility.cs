using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for long.
	/// </summary>
	public static class LongUtility
	{
		/// <summary>
		/// Determines whether all the texts in the provided collection represent a valid long.
		/// </summary>
		/// <param name="texts">A collection of texts.</param>
		public static bool AreLongs(IEnumerable<string> texts)
		{
			return texts.All(s => IsLong(s));
		}

		/// <summary>
		/// Determines whether the provided text represents a valid long.
		/// </summary>
		/// <param name="text">The text.</param>
		public static bool IsLong(string text)
		{
			return long.TryParse(text, out long value);
		}

		/// <summary>
		/// Converts a collection of string representations of longs to actual longs.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<long> Parse(IEnumerable<string> texts)
		{
			return texts.Select(t => long.Parse(t));
		}

		/// <summary>
		/// Converts a collection of string representations of longs to actual longs. Sets zero if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<long> Parse0(IEnumerable<string> texts)
		{
			return texts.Select(t => Parse0(t));
		}

		/// <summary>
		/// Parses the provided text to a long. Returns zero if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static long Parse0(string text)
		{
			return ParseNullable(text) ?? 0;
		}

		/// <summary>
		/// Converts a collection of string representations of longs to actual longs. Sets null if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<long?> ParseNullable(IEnumerable<string> texts)
		{
			return texts.Select(t => ParseNullable(t));
		}

		/// <summary>
		/// Parses the provided text to a long. Returns null if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static long? ParseNullable(string text)
		{
			if(long.TryParse(text, out long value)) {
				return value;
			}
			return null;
		}
	}
}
