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
/// Utilities for <see cref="IList{T}"/>.
/// </summary>
public static class IListUtility
{
	/// <summary>
	/// Replaces all occurrences of the specified value in this list with the specified replacement value.
	/// </summary>
	/// <remarks>This method iterates through the list and replaces all elements that are equal to <paramref name="valueToReplace"/> with <paramref name="replacement"/>. Equality is determined using the default equality comparer for the type <typeparamref name="T"/>.</remarks>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	/// <param name="list">The list in which the replacement will occur. Cannot be <see langword="null"/>.</param>
	/// <param name="valueToReplace">The value to be replaced in the list.</param>
	/// <param name="replacement">The value to replace <paramref name="valueToReplace"/> with.</param>
	public static void Replace<T>(this IList<T> list, T valueToReplace, T replacement)
	{
		DefensiveUtility.ThrowIfNull(list, nameof(list));
		for(int i = list.Count - 1; i >= 0; --i) {
			if(EqualityComparer<T>.Default.Equals(list[i], valueToReplace)) {
				list[i] = replacement;
			}
		}
	}
}
