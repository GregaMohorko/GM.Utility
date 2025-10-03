/*
MIT License

Copyright (c) 2025 Gregor Mohorko

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
Created: 2025-10-03
Author: Gregor Mohorko
*/

using System.Collections.Generic;

namespace GM.Utility;
/// <summary>
/// Utilities for <see cref="ICollection{T}"/>.
/// </summary>
public static class ICollectionUtility
{
	/// <summary>
	/// Removes the elements of the specified collection from this collection (calls the <see cref="ICollection{T}.Remove(T)"/> method for each element in the specified collection).
	/// </summary>
	/// <typeparam name="T">The type of the elements.</typeparam>
	/// <param name="collection">The collection from which to remove elements.</param>
	/// <param name="elementsToRemove">The collection whose elements should be removed from this collection. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
	public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> elementsToRemove)
	{
		DefensiveUtility.ThrowIfNull(collection, nameof(collection));
		DefensiveUtility.ThrowIfNull(elementsToRemove, nameof(elementsToRemove));

		foreach(T item in elementsToRemove) {
			collection.Remove(item);
		}
	}
}
