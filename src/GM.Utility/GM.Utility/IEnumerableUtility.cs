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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="IEnumerable{T}"/>.
	/// <para>
	/// For more features, check out https://github.com/morelinq/MoreLINQ
	/// </para>
	/// </summary>
	public static class IEnumerableUtility
	{
		/// <summary>
		/// Determines whether all elements in this collection produce the same value with the provided value selector. Compares using the <see cref="object.Equals(object)"/> method.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <typeparam name="TValue">The type of the values to compare.</typeparam>
		/// <param name="enumerable">The collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element to select the value on which to compare elements.</param>
		public static bool AllSame<T,TValue>(this IEnumerable<T> enumerable,Func<T, TValue> valueSelector)
		{
			if(enumerable == null) {
				throw new ArgumentNullException(nameof(enumerable));
			}
			if(valueSelector == null) {
				throw new ArgumentNullException(nameof(valueSelector));
			}
			if(!enumerable.Any()) {
				return true;
			}
			TValue sample = valueSelector(enumerable.First());
			return enumerable.All(e => sample.Equals(valueSelector(e)));
		}

		/// <summary>
		/// Returns distinct elements from a sequence using the provided value selector for equality comparison.
		/// </summary>
		/// <typeparam name="T1">The type of the elements of source.</typeparam>
		/// <typeparam name="TValue">The type of the value on which to distinct.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element to select the value on which to distinct.</param>
		public static IEnumerable<T1> DistinctBy<T1, TValue>(this IEnumerable<T1> source, Func<T1, TValue> valueSelector)
		{
			var alreadyIncluded = new HashSet<TValue>();

			foreach(var element in source) {
				if(alreadyIncluded.Add(valueSelector(element))) {
					yield return element;
				}
			}
		}

		/// <summary>
		/// Determines whether the provided collection is null or is empty.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the collection.</typeparam>
		/// <param name="enumerable">The collection, which may be null or empty.</param>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			// Solution from: http://danielvaughan.org/post/IEnumerable-IsNullOrEmpty.aspx
			if(enumerable == null) {
				return true;
			}

			// if this is a collection, use the Count property
			// the count property is O(1) while IEnumerable.Count() is O(N)
			if(enumerable is ICollection<T> collection) {
				return collection.Count < 1;
			}
			return enumerable.Any();
		}

		/// <summary>
		/// Orders this collection using the keys that will be extracted using the provided key selectors.
		/// <para>Key selectors are <see cref="Tuple{T1, T2}"/> where the first item is a function that selects the key and the second item determines whether to order ascendingly (if true) or descendingly (if false).</para>
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <param name="collection">The collection to order.</param>
		/// <param name="keySelectors">The key selectors that will be used to extract the keys on which to order from the elements.</param>
		public static IOrderedEnumerable<T> OrderByMultipleKeys<T>(this IEnumerable<T> collection, IEnumerable<Tuple<Func<T, object>, bool>> keySelectors)
		{
			int sortKeySelectorsCount = keySelectors.Count();

			if(sortKeySelectorsCount == 0) {
				throw new NotImplementedException("Empty SortKeySelector list provided.");
			}

			IOrderedEnumerable<T> orderedList;

			Tuple<Func<T, object>, bool> tuple = keySelectors.ElementAt(0);

			Func<T, object> sortKeySelector = tuple.Item1;

			if(tuple.Item2) {
				orderedList = collection.OrderBy(sortKeySelector);
			} else {
				orderedList = collection.OrderByDescending(sortKeySelector);
			}

			for(int i = 1; i < sortKeySelectorsCount; ++i) {
				tuple = keySelectors.ElementAt(i);
				sortKeySelector = tuple.Item1;

				if(tuple.Item2) {
					orderedList = orderedList.ThenBy(sortKeySelector);
				} else {
					orderedList = orderedList.ThenByDescending(sortKeySelector);
				}
			}

			return orderedList;
		}

		/// <summary>
		/// Rotates this collection (moves the start by the specified number of elements). Can be negative.
		/// <para>Example: If your collection is { 1, 2, 3, 4 } and you rotate by 1, result will be { 4, 1, 2, 3 }.</para>
		/// <para>Example 2: If your collection is { 1, 2, 3, 4, 5 } and you rotate by -2, result will be { 3, 4, 5, 1, 2 }.</para>
		/// </summary>
		/// <typeparam name="T">The element type..</typeparam>
		/// <param name="collection">The collection to rotate.</param>
		/// <param name="numberOfElements">The amount of elements to rotate.</param>
		public static IEnumerable<T> Rotate<T>(this IEnumerable<T> collection, int numberOfElements)
		{
			if(numberOfElements == 0) {
				return collection;
			}
			return RotateImpl(collection, numberOfElements);
		}

		private static IEnumerable<T> RotateImpl<T>(IEnumerable<T> collection, int numberOfElements)
		{
			List<T> list = collection.ToList();

			if(numberOfElements < 0) {
				numberOfElements = (numberOfElements % list.Count) + list.Count;
			}
			if(numberOfElements >= list.Count) {
				numberOfElements %= list.Count;
			}

			int start = list.Count - numberOfElements;
			for(int i = start; i < list.Count; ++i) {
				yield return list[i];
			}
			for(int i = 0; i < start; ++i) {
				yield return list[i];
			}
		}

		/// <summary>
		/// Determines whether two sequences are equal by comparing the elements by using the default equality comparer for their type. Note that this method does not compare the order of elements and returns true even if the order is not equal.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">The first sequence.</param>
		/// <param name="second">An IEnumerable to compare to the first sequence.</param>
		public static bool SequenceEqualUnordered<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			// Solution from: http://stackoverflow.com/questions/22173762/check-if-two-lists-are-equal
			// linear time!

			// hash-based dictionary of counts
			Dictionary<T, int> counts = first
				.GroupBy(element => element)
				.ToDictionary(group => group.Key, group => group.Count());

			// -1 for each element of the second sequence
			bool ok = true;
			foreach(T element in second) {
				if(counts.ContainsKey(element)) {
					counts[element]--;
				} else {
					ok = false;
					break;
				}
			}

			// check if the resultant counts are all zeros
			return ok && counts.Values.All(count => count == 0);
		}

		/// <summary>
		/// Creates a <see cref="Dictionary{TKey, TValue}"/> from a grouping collection.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TElement">The type of the elements.</typeparam>
		/// <param name="groupingCollection">An <see cref="IEnumerable{T}"/> of <see cref="IGrouping{TKey, TElement}"/> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
		public static Dictionary<TKey, List<TElement>> ToDictionaryFromGrouping<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> groupingCollection)
		{
			return groupingCollection.ToDictionary(g => g.Key, g => g.ToList());
		}
	}
}
