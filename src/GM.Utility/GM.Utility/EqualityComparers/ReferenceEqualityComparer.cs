/*
MIT License

Copyright (c) 2019 Grega Mohorko

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
Created: 2019-01-24
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility.EqualityComparers
{
	/// <summary>
	/// Equality comparer that uses the <see cref="object.ReferenceEquals(object, object)"/> to compare values.
	/// </summary>
	public class ReferenceEqualityComparer : EqualityComparer<object>
	{
		/// <summary>
		/// Determines whether the provided objects are the same reference.
		/// </summary>
		/// <param name="x">First object.</param>
		/// <param name="y">Second object.</param>
		public override bool Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}

		/// <summary>
		/// Returns the hash code of the provided object.
		/// </summary>
		/// <param name="obj">The object.</param>
		public override int GetHashCode(object obj)
		{
			if(obj == null) {
				return 0;
			}

			return obj.GetHashCode();
		}
	}
}
