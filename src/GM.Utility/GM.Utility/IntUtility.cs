using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class IntUtility
	{
		/// <summary>
		/// Determines whether the provided string is a valid integer.
		/// </summary>
		public static bool IsInt(string text)
		{
			int integer;
			if(int.TryParse(text, out integer))
				return true;

			return false;
		}

		/// <summary>
		/// Determines whether all the strings in the provided collection are a valid integer.
		/// </summary>
		public static bool AreInts(IEnumerable<string> texts)
		{
			return texts.All(s => IsInt(s));
		}

		/// <summary>
		/// Converts a collection of string representations of numbers to its 32-bit signed integer equivalents.
		/// </summary>
		public static IEnumerable<int> Parse(IEnumerable<string> texts)
		{
			return texts.Select(s => int.Parse(s));
		}

		/// <summary>
		/// Returns null if the provided text is not a valid integer.
		/// </summary>
		public static int? ParseNullable(string text)
		{
			int value;
			if(int.TryParse(text, out value))
				return value;

			return null;
		}

		/// <summary>
		/// Returns zero if the provided text is empty.
		/// </summary>
		public static int Parse0(string text)
		{
			if(text == string.Empty)
				return 0;

			return int.Parse(text);
		}
	}
}
