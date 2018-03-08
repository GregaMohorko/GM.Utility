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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for string.
	/// </summary>
	public static class StringUtility
	{
		/// <summary>
		/// Converts the specified string to a stream that can be read from.
		/// </summary>
		/// <param name="value">A string value that the stream will read from.</param>
		public static MemoryStream ConvertToStream(string value)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(value);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		/// <summary>
		/// Indents this text by the specified space count.
		/// </summary>
		/// <param name="text">The text to indent.</param>
		/// <param name="spaceCount">The number of spaces to indent.</param>
		public static string Indent(this string text, int spaceCount)
		{
			string indentation = new string(' ', spaceCount);
			string newLineReplacement = Environment.NewLine + indentation;
			return indentation + text.Replace(Environment.NewLine, newLineReplacement);
		}

		/// <summary>
		/// Returns a new string in which all occurences of the specified value are removed.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="value">The string to seek and remove.</param>
		public static string RemoveAllOf(this string text, string value)
		{
			return text.Replace(value, string.Empty);
		}

		/// <summary>
		/// Returns a new string in which the first occurence of the specified value is removed.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="value">The string to seek and remove its first occurence.</param>
		public static string RemoveFirstOf(this string text, string value)
		{
			int index = text.IndexOf(value);
			if(index < 0) {
				// the value is not present in the text
				return text;
			}

			return text.Remove(index, value.Length);
		}

		/// <summary>
		/// Removes the whitespace in this text.
		/// </summary>
		/// <param name="text">The text.</param>
		public static string RemoveWhitespace(this string text)
		{
			return Regex.Replace(text, @"\s+", "");
		}

		/// <summary>
		/// Converts this string to a stream that can be read from.
		/// </summary>
		/// <param name="value">A string value that the stream will read from.</param>
		public static MemoryStream ToStream(this string value)
		{
			return ConvertToStream(value);
		}

		/// <summary>
		/// Transforms the first letter into upper case.
		/// </summary>
		/// <param name="text">The text.</param>
		public static string ToUpperFirstLetter(this string text)
		{
			if(text.Length == 0)
				return text;
			
			text = char.ToUpper(text[0]) + text.Substring(1);

			return text;
		}
	}
}
