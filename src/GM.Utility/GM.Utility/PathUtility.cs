/*
MIT License

Copyright (c) 2018 Grega Mohorko

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
Created: 2018-3-9
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for operations on <see cref="string"/> instances that contain file or directory path information. These operations are performed in a cross-platform manner.
	/// <para>This class offers utilities that can be used with the <see cref="Path"/> class.</para>
	/// </summary>
	public static class PathUtility
	{
		/// <summary>
		/// Converts the specified name to a valid file name.
		/// <para>Any invalid characters are replaced with '_'.</para>
		/// <para>Spaces are removed.</para>
		/// </summary>
		/// <param name="name">The name to convert.</param>
		public static string GetSafeFileName(string name)
		{
			var buffer = new StringBuilder(name);

			char[] invalidChars = Path.GetInvalidFileNameChars();
			foreach(char c in invalidChars) {
				buffer = buffer.Replace(c, '_');
			}
			buffer[0] = char.ToUpperInvariant(buffer[0]);
			buffer = buffer.Replace(" ", "");

			return buffer.ToString();
		}

		/// <summary>
		/// Converts the specified name to a valid file name (without extension).
		/// <para>Any invalid characters (including dots) are replaced with '_'.</para>
		/// <para>Spaces are removed.</para>
		/// </summary>
		/// <param name="name">The name to convert.</param>
		public static string GetSafeFileNameWithoutExtension(string name)
		{
			return GetSafeFileName(name).Replace('.', '_');
		}

		private static readonly Regex safeFileNameValidIdentifier = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$", RegexOptions.Compiled | RegexOptions.Singleline);

		/// <summary>
		/// Determines whether the provided name is a valid file name.
		/// </summary>
		/// <param name="name">The name to check.</param>
		public static bool IsSafeFileName(string name)
		{
			if(string.IsNullOrWhiteSpace(name)) {
				return false;
			}

			if(!safeFileNameValidIdentifier.IsMatch(name)) {
				return false;
			}

			char[] invalidChars = Path.GetInvalidFileNameChars();
			foreach(char c in name) {
				if(invalidChars.Contains(c)) {
					return false;
				}
			}

			return true;
		}
	}
}
