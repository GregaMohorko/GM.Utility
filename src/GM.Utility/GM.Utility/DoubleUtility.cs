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
	/// Utilities for double.
	/// </summary>
	public static class DoubleUtility
	{
		/// <summary>
		/// Determines whether all the texts in the provided collection represent a valid double.
		/// </summary>
		/// <param name="texts">A collection of texts.</param>
		public static bool AreDoubles(IEnumerable<string> texts)
		{
			return texts.All(t => IsDouble(t));
		}

		/// <summary>
		/// Determines whether the provided text represents a valid double.
		/// </summary>
		/// <param name="text">The text.</param>
		public static bool IsDouble(string text)
		{
			return double.TryParse(text, out double value);
		}

		/// <summary>
		/// Converts a collection of string representations of doubles to actual doubles.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<double> Parse(IEnumerable<string> texts)
		{
			return texts.Select(t => double.Parse(t));
		}

		/// <summary>
		/// Converts a collection of string representations of doubles to actual doubles. Sets zero if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<double> Parse0(IEnumerable<string> texts)
		{
			return texts.Select(t => Parse0(t));
		}

		/// <summary>
		/// Parses the provided text to a double. Returns zero if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static double Parse0(string text)
		{
			return ParseNullable(text) ?? 0;
		}

		/// <summary>
		/// Converts a collection of string representations of doubles to actual doubles. Sets null if a text is invalid.
		/// </summary>
		/// <param name="texts">A collection of texts to parse.</param>
		public static IEnumerable<double?> ParseNullable(IEnumerable<string> texts)
		{
			return texts.Select(t => ParseNullable(t));
		}

		/// <summary>
		/// Parses the provided text to a double. Returns null if it is invalid.
		/// </summary>
		/// <param name="text">The text to parse.</param>
		public static double? ParseNullable(string text)
		{
			if(double.TryParse(text,out double value)) {
				return value;
			}
			return null;
		}
	}
}
