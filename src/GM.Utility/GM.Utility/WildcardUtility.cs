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

Project: GM.Utility.Test
Created: 2021-01-10
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for working with strings containing wildcards (*).
	/// </summary>
	public static class WildcardUtility
	{
		/// <summary>
		/// Inserts the provided values into the specified text with wildcards. The number of wildcards in the text and the number of provided values must exactly match.
		/// </summary>
		/// <param name="textWithWildcards">The text containg wildcards into witch the values should be inserted.</param>
		/// <param name="values">The values to insert.</param>
		public static string Insert(string textWithWildcards, string[] values)
		{
			if(textWithWildcards == null) {
				throw new ArgumentNullException(nameof(textWithWildcards));
			}
			if(values == null) {
				throw new ArgumentNullException(nameof(values));
			}

			int wildcardCount = textWithWildcards.Count(c => c == '*');

			if(wildcardCount != values.Length) {
				throw new ArgumentOutOfRangeException(nameof(values), $"The text '{textWithWildcards}' contains {wildcardCount} wildcards, but you provided an array with {values.Length} values.");
			}

			if(wildcardCount == 0) {
				return textWithWildcards;
			}

			string result = textWithWildcards;
			int currentPos = result.Length - 1;
			for(int i = wildcardCount - 1; i >= 0; --i) {
				currentPos = result.LastIndexOf('*', currentPos);
				result = result.Substring(0, currentPos) + values[i] + result.Substring(1+currentPos);
			}
			return result;
		}

		/// <summary>
		/// Escapes the provided wildcard pattern and then replaces '*' with '.*' and '?' with '.'.
		/// <para>Also surrounds the pattern in ^...$.</para>
		/// </summary>
		/// <param name="pattern">A regular expression with possible wildcards (*).</param>
		public static string WildcardToRegex(string pattern)
		{
			return Regex.Escape(pattern)
				.Replace(@"\*", ".*")
				.Replace(@"\?", ".");
		}
	}
}
