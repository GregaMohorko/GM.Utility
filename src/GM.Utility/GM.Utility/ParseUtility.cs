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
Created: 2020-6-28
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for parsing string.
	/// </summary>
	public static class ParseUtility
	{
		/// <summary>
		/// Converts the string representation of <typeparamref name="T"/> to its <typeparamref name="T"/> equivalent.
		/// <para>Uses <see cref="Convert.ChangeType(object, Type)"/>.</para>
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="value">A string containing the value to convert.</param>
		/// <exception cref="InvalidCastException" />
		/// <exception cref="FormatException" />
		public static T ParseTo<T>(this string value)
		{
			return (T)Convert.ChangeType(value, typeof(T));
		}

		/// <summary>
		/// Tries to convert the specified string representation of <typeparamref name="T"/> to its <typeparamref name="T"/> equivalent. A return value indicates whether the conversion succeeded or failed.
		/// </summary>
		/// <typeparam name="T">The type of object to return.</typeparam>
		/// <param name="value">A string containing the value to convert.</param>
		/// <param name="result">The result of the parse operation.</param>
		public static bool TryParseTo<T>(this string value, out T result)
		{
			try {
				result = ParseTo<T>(value);
			} catch(Exception e) {
				if(e is InvalidCastException || e is FormatException) {
					result = default;
					return false;
				}
				throw;
			}
			return true;
		}
	}
}
