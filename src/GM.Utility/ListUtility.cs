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
Created: 2019-09-17
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="List{T}"/>.
	/// </summary>
	public static class ListUtility
	{
		/// <summary>
		/// Removes the elements of the specified collection from this list (calls the <see cref="List{T}.Remove(T)"/> method for each element in the specified collection).
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="list">The list.</param>
		/// <param name="collection">The collection whose elements should be removed from this list. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
		public static void RemoveRange<T>(this List<T> list, IEnumerable<T> collection)
		{
			if(list == null) {
				throw new ArgumentNullException(nameof(list));
			}
			if(collection == null) {
				throw new ArgumentNullException(nameof(collection));
			}

			foreach(T item in collection) {
				list.Remove(item);
			}
		}
	}
}
