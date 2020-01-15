/*
MIT License

Copyright (c) 2020 Gregor Mohorko

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
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="string"/>.
	/// </summary>
	public static class StringUtility
	{
		private static readonly Lazy<Regex> REGEX_WHITESPACE = new Lazy<Regex>(() => new Regex(@"\s+", RegexOptions.Compiled));
		/// <summary>
		/// Regex with pattern @"\s+".
		/// </summary>
		public static Regex RegexWhitespace => REGEX_WHITESPACE.Value;

		/// <summary>
		/// Converts the specified string to a stream by using UTF-8 encoding that can be read from.
		/// <para>Don't forget to dispose the stream.</para>
		/// </summary>
		/// <param name="value">A string value that the stream will read from.</param>
		public static MemoryStream ConvertToStream(string value)
		{
#pragma warning disable IDE0067 // Dispose objects before losing scope
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(value);
			writer.Flush();
			stream.Position = 0;
			return stream;
#pragma warning restore IDE0067 // Dispose objects before losing scope
		}

		/// <summary>
		/// Determines whether this text contains any whitespace.
		/// </summary>
		/// <param name="text">The text to determine whether it contains any whitespace.</param>
		public static bool ContainsWhitespace(this string text)
		{
			return RegexWhitespace.IsMatch(text);
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
			return RegexWhitespace.Replace(text, "");
		}

		/// <summary>
		/// Returns a new string in which the substring in the specified range (inclusive) is replaced with the provided replacement string.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="startIndex">The zero-based starting position of the substring to replace (inclusive).</param>
		/// <param name="endIndex">The zero-based ending position of the substring to replace (inclusive).</param>
		/// <param name="replacement">The string with which to replace the specified range.</param>
		public static string Replace(this string text, int startIndex, int endIndex, string replacement)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(replacement == null) {
				throw new ArgumentNullException(nameof(replacement));
			}
			if(endIndex < startIndex) {
				throw new ArgumentOutOfRangeException(nameof(endIndex), "The end index must be bigger than start index.");
			}

			return text.Substring(0, startIndex) + replacement + text.Substring(endIndex + 1, text.Length - endIndex - 1);
		}

		/// <summary>
		/// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string. The comparison is done in a case insensitive (invariant culture) way.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="oldValue">The string to be replaced.</param>
		/// <param name="newValue">The string to replace all occurrences of oldValue.</param>
		/// <returns></returns>
		public static string ReplaceCaseInsensitive(this string text, string oldValue, string newValue)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(oldValue == null) {
				throw new ArgumentNullException(nameof(oldValue));
			}
			if(newValue == null) {
				throw new ArgumentNullException(nameof(newValue));
			}
			if(oldValue.Length == 0) {
				throw new ArgumentException("The old value must not be an empty string.", nameof(oldValue));
			}

			string textLower = text.ToLowerInvariant();
			string oldValueLower = oldValue.ToLowerInvariant();

			for(int i = text.Length - oldValue.Length; i >= 0; --i) {
				if(textLower.Substring(i, oldValue.Length) == oldValueLower) {
					text = text.Replace(i, i + oldValue.Length - 1, newValue);
				}
			}

			return text;
		}

		/// <summary>
		/// Returns a new string from the provided text with the specified maximum length. If the original text is longer than the specified maximum length, 3 dots (...) are added.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="maxLength">The maximum length of the returned string (excluding the 3 dots).</param>
		public static string ShortenWith3Dots(this string text, int maxLength)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(maxLength < 1) {
				throw new ArgumentOutOfRangeException(nameof(maxLength), "The maximum length must not be less than 1.");
			}

			if(text.Length <= maxLength) {
				return text;
			}

			return text.Substring(0, maxLength) + "...";
		}

		private static readonly Lazy<Regex> REGEX_SENTENCEORTITLE = new Lazy<Regex>(() => new Regex("(^| )[A-z]", RegexOptions.Compiled));

		/// <summary>
		/// Converts this string into PascalCase.
		/// </summary>
		/// <param name="text">The text to transform to PascalCase.</param>
		public static string ToPascalCase(this string text)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(text.Length == 0) {
				return "";
			}
			string textLower = text.ToLowerInvariant();
			// leave out the space before the 2nd word, and transform the first letter of it to upper case
			return REGEX_SENTENCEORTITLE.Value.Replace(textLower, m => $"{char.ToUpperInvariant(m.Value.Last())}");
		}

		private static readonly Lazy<Regex> REGEX_PASCAL = new Lazy<Regex>(() => new Regex("[a-z][A-Z]", RegexOptions.Compiled));

		/// <summary>
		/// Converts this string from PascalCase to Sentence case.
		/// </summary>
		/// <param name="text">The text in PascalCase to transform to Sentence case.</param>
		public static string ToSentenceCase(this string text)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(text.Length == 0) {
				return "";
			}
			// add a space between and transform the first letter of the 2nd word to lower case
			return REGEX_PASCAL.Value.Replace(text, m => $"{m.Value[0]} {char.ToLowerInvariant(m.Value[1])}");
		}

		/// <summary>
		/// Converts this string to a stream by using UTF-8 encoding that can be read from.
		/// </summary>
		/// <param name="value">A string value that the stream will read from.</param>
		public static MemoryStream ToStream(this string value)
		{
			return ConvertToStream(value);
		}

		/// <summary>
		/// Converts this string to Title Case (except for words that are entirely in uppercase, which are considered to be acronyms) using the invariant culture.
		/// </summary>
		/// <param name="text">The text.</param>
		public static string ToTitleCase(this string text)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(text.Length == 0) {
				return "";
			}
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			return textInfo.ToTitleCase(text);
		}

		/// <summary>
		/// Transforms the first letter into upper case.
		/// </summary>
		/// <param name="text">The text.</param>
		public static string ToUpperFirstLetter(this string text)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(text.Length == 0) {
				return "";
			}

			text = char.ToUpper(text[0]) + text.Substring(1);

			return text;
		}

		/// <summary>
		/// Transforms the first letter into upper case and all other to lower case.
		/// </summary>
		/// <param name="text">The text.</param>
		public static string ToUpperFirstLetterOnly(this string text)
		{
			if(text == null) {
				throw new ArgumentNullException(nameof(text));
			}
			if(text.Length == 0) {
				return "";
			}

			text = char.ToUpperInvariant(text[0]) + text.Substring(1).ToLowerInvariant();

			return text;
		}
	}
}
