/*
MIT License

Copyright (c) 2022 Gregor Mohorko

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

Project: GM.WPF
Created: 2022-11-1
Author: Gregor Mohorko
*/

using System;
using System.Globalization;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="ValueType"/>.
	/// </summary>
	public static class ValueTypeUtility
	{
		/// <summary>
		/// Returns true if this nullable value is null or if it equals to zero.
		/// <para>The zero equality check is done with <see cref="ValueType.Equals(object)"/>.</para>
		/// </summary>
		/// <typeparam name="T">The nullable (number) type.</typeparam>
		/// <param name="value">The nullable (number) value.</param>
		public static bool IsNullOrZero<T>(this T? value) where T : struct, IConvertible
		{
			return value == null
				|| value.Value.ToInt32(CultureInfo.CurrentCulture).Equals(0);
		}
	}
}
