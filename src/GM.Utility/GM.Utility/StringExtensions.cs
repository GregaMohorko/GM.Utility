using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class StringExtensions
	{
		public static string RemoveWhitespace(this string text)
		{
			return Regex.Replace(text, @"\s+", "");
		}

		public static string ToUpperFirstLetter(this string text)
		{
			if(text.Length == 0)
				return text;

			//if(char.IsLetter(text[0]))
			text = char.ToUpper(text[0]) + text.Substring(1);

			return text;
		}
	}
}
