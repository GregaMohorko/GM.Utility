using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class BoolUtility
	{
		/// <summary>
		/// Returns null if the provided text is not a valid bool.
		/// </summary>
		public static bool? ParseNullable(string text)
		{
			bool value;
			if(TryParse(text, out value))
				return value;

			return null;
		}

		/// <summary>
		/// Valid values: true, false, 0, 1.
		/// </summary>
		public static bool TryParse(string value, out bool result)
		{
			result = false;

			if(value == null) {
				return false;
			}

			if(value == bool.FalseString) {
				return true;
			}
			if(value == bool.TrueString) {
				result = true;
				return true;
			}

			switch(value.ToLower()) {
				case "true":
				case "1":
					result = true;
					return true;
				case "false":
				case "0":
				case "":
					return true;
			}

			return false;
		}
	}
}
