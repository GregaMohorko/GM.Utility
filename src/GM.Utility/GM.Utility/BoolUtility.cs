/*
MIT License

Copyright (c) 2017 Grega Mohorko

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
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for boolean.
	/// </summary>
	public static class BoolUtility
	{

		/// <summary>
		/// Determines whether all the texts in the provided collection represent a valid bool.
		/// </summary>
		/// <param name="texts">A collection of texts.</param>
		public static bool AreBools(IEnumerable<string> texts)
		{
			return texts.All(t => IsBool(t));
		}

		/// <summary>
		/// Determines whether the provided text represents a valid bool.
		/// </summary>
		/// <param name="text">The text.</param>
		public static bool IsBool(string text)
		{
			return bool.TryParse(text, out bool value);
		}

		/// <summary>
		/// Converts a collection of string representations of bools to actual bools.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<bool> Parse(IEnumerable<string> texts)
		{
			return texts.Select(t => bool.Parse(t));
		}

		/// <summary>
		/// Converts a collection of string representations of bools to actual bools. Sets null if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<bool?> ParseNullable(IEnumerable<string> texts)
		{
			return texts.Select(t => ParseNullable(t));
		}

		/// <summary>
		/// Parses the provided text to a bool. Returns null if it is invalid.
		/// <para>
		/// Valid values: true, false, 0, 1, empty string.
		/// </para>
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static bool? ParseNullable(string text)
		{
			if(TryParse(text, out bool value)) {
				return value;
			}
			return null;
		}

		/// <summary>
		/// Attempts to parse the provided text and returns true, if it was successful.
		/// <para>
		/// Valid values: true, false, 0, 1, empty string.
		/// </para>
		/// </summary>
		/// <param name="text">The text to parse.</param>
		/// <param name="result">Will contain true or false.</param>
		public static bool TryParse(string text, out bool result)
		{
			result = false;

			if(text == null) {
				return false;
			}

			if(text == bool.FalseString) {
				return true;
			}
			if(text == bool.TrueString) {
				result = true;
				return true;
			}

			switch(text.ToLower()) {
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
