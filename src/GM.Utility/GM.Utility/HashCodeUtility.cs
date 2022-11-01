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

Project: GM.Utility
Created: 2021-11-02
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for calculating the hash code when overriding <see cref="object.GetHashCode"/>.
	/// </summary>
	public static class HashCodeUtility
	{
		/// <summary>
		/// Calculates the hash code from the provided values.
		/// <para>The order of the values is important.</para>
		/// <para>Values can be null.</para>
		/// </summary>
		/// <param name="value">The first value.</param>
		/// <param name="additionalValues">Any additional values to include in the calculation.</param>
		public static int GetHashCode(object value, params object[] additionalValues)
		{
			object[] values = Util.CombineWithParams(value, additionalValues);

			// https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode

			unchecked {
				int hash = (int)2166136261;
				foreach(object val in values) {
					hash = (hash * 16777619) ^ (val?.GetHashCode() ?? 0);
				}
				return hash;
			}
		}
	}
}
