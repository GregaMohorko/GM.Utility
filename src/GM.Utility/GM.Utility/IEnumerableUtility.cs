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
Created: 2017-10-27
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		/// Determines whether all elements in this collection are equal to each other. Compares using the <see cref="object.Equals(object)"/> method.
		/// <para>This method returns true if the collection is empty or it contains only one element.</para>
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="enumerable">The collection.</param>
		public static bool AllSame<T>(this IEnumerable<T> enumerable)
		{
			if(enumerable == null) {
				throw new ArgumentNullException(nameof(enumerable));
			}
			if(!enumerable.Any()) {
				return true;
			}
			T first = enumerable.First();
			return enumerable.Skip(1).All(e => e.Equals(first));
		}

		/// <summary>
		/// Determines whether all elements in this collection produce the same value with the provided value selector. Compares using the <see cref="object.Equals(object)"/> method.
		/// <para>This method returns true if the collection is empty or it contains only one element.</para>
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <typeparam name="TValue">The type of the values to compare.</typeparam>
		/// <param name="enumerable">The collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element to select the value on which to compare elements.</param>
		public static bool AllSame<T, TValue>(this IEnumerable<T> enumerable, Func<T, TValue> valueSelector)
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
			Func<T, bool> predicate;
			if(sample == null) {
				predicate = e => valueSelector(e) == null;
			} else {
				predicate = e => sample.Equals(valueSelector(e));
			}
			return enumerable.Skip(1).All(predicate);
		}

		/// <summary>
		/// Determines whether all elements in this collection produce the same values with the provided value selectors. Compares using the <see cref="object.Equals(object)"/> method.
		/// <para>This method returns true if the collection is empty or it contains only one element.</para>
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <typeparam name="TValue1">The type of the first values to compare.</typeparam>
		/// <typeparam name="TValue2">The type of the second values to compare.</typeparam>
		/// <param name="enumerable">The collection.</param>
		/// <param name="valueSelector1">A transform function to apply to each element to select the first value on which to compare elements.</param>
		/// <param name="valueSelector2">A transform function to apply to each element to select the second value on which to compare elements.</param>
		public static bool AllSame<T, TValue1, TValue2>(this IEnumerable<T> enumerable, Func<T, TValue1> valueSelector1, Func<T, TValue2> valueSelector2)
		{
			if(enumerable == null) {
				throw new ArgumentNullException(nameof(enumerable));
			}
			if(valueSelector1 == null) {
				throw new ArgumentNullException(nameof(valueSelector1));
			}
			if(valueSelector2 == null) {
				throw new ArgumentNullException(nameof(valueSelector2));
			}
			if(!enumerable.Any()) {
				return true;
			}
			T first = enumerable.First();
			TValue1 sample1 = valueSelector1(first);
			TValue2 sample2 = valueSelector2(first);
			return enumerable.All(e => sample1.Equals(valueSelector1(e)) && sample2.Equals(valueSelector2(e)));
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
		/// Performs the specified action on each element of this collection.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="collection">The source collection.</param>
		/// <param name="action">The action to perform on each element of this collection.</param>
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			if(collection == null) {
				throw new ArgumentNullException(nameof(collection));
			}
			if(action == null) {
				throw new ArgumentNullException(nameof(action));
			}

			foreach(T element in collection) {
				action(element);
			}
		}

		/// <summary>
		/// Returns the first element in a sequence whose provided transform function returns a max value in the provided collection.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the collection.</typeparam>
		/// <param name="collection">The source collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element that returns the value to compare items with.</param>
		public static T FirstMax<T>(this IEnumerable<T> collection, Func<T, int> valueSelector)
		{
			List<T> list = collection.ToList();

			if(list.Count == 0) {
				// will throw exception as it should anyway :)
				return collection.First();
			}
			if(list.Count == 1) {
				return list.First();
			}

			T itemWithMax = list[0];
			int max = valueSelector(list[0]);
			for(int i = 1; i < list.Count; ++i) {
				T item = list[i];
				int current = valueSelector(item);
				if(current > max) {
					max = current;
					itemWithMax = item;
				}
			}
			return itemWithMax;
		}

		/// <summary>
		/// Invokes a transform function on each element of a sequence and returns the maximum <see cref="int"/> value, or zero if the sequence contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <param name="collection">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		public static int MaxOrZero<T>(this IEnumerable<T> collection, Func<T, int> selector)
		{
			if(collection == null) {
				throw new ArgumentNullException(nameof(collection));
			}
			if(!collection.Any()) {
				return 0;
			}
			return collection.Max(selector);
		}

		/// <summary>
		/// Returns the first element in a sequence whose provided transform function returns a min value in the provided collection.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the collection.</typeparam>
		/// <param name="collection">The source collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element that returns the value to compare items with.</param>
		public static T FirstMin<T>(this IEnumerable<T> collection, Func<T, int> valueSelector)
		{
			List<T> list = collection.ToList();

			if(list.Count == 0) {
				// will throw exception as it should anyway :)
				return collection.First();
			}
			if(list.Count == 1) {
				return list.First();
			}

			T itemWithMin = list[0];
			int min = valueSelector(list[0]);
			for(int i = 1; i < list.Count; ++i) {
				T item = list[i];
				int current = valueSelector(item);
				if(current < min) {
					min = current;
					itemWithMin = item;
				}
			}
			return itemWithMin;
		}

		/// <summary>
		/// Determines whether the provided collection is null or empty.
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
			return enumerable.Any() == false;
		}

		/// <summary>
		/// Returns the minimum value in a generic sequence, or a default value if the sequence contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="sequence">The sequence.</param>
		public static T MinOrDefault<T>(this IEnumerable<T> sequence)
		{
			return sequence.Any()
				? sequence.Min()
				: default;
		}

		/// <summary>
		/// Invokes a transform function on each element of a generic sequence and returns the minimum resulting value, or a default value if the sequence contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
		/// <param name="sequence">A sequence of values to determine the minimum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		public static TResult MinOrDefault<T, TResult>(
			this IEnumerable<T> sequence,
			Func<T, TResult> selector
			)
		{
			return sequence.Any()
				? sequence.Min(selector)
				: default;
		}

		/// <summary>
		/// Returns the maximum value in a generic sequence, or a default value if the sequence contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <param name="sequence">The sequence.</param>
		public static T MaxOrDefault<T>(this IEnumerable<T> sequence)
		{
			return sequence.Any()
				? sequence.Max()
				: default;
		}

		/// <summary>
		/// Invokes a transform function on each element of a generic sequence and returns the maximum resulting value, or a default value if the sequence contains no elements.
		/// </summary>
		/// <typeparam name="T">The type of the elements.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
		/// <param name="sequence">A sequence of values to determine the maximum value of.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		public static TResult MaxOrDefault<T, TResult>(
			this IEnumerable<T> sequence,
			Func<T, TResult> selector
			)
		{
			return sequence.Any()
				? sequence.Max(selector)
				: default;
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
			if(collection == null) {
				throw new ArgumentNullException(nameof(collection));
			}
			if(keySelectors == null) {
				throw new ArgumentNullException(nameof(keySelectors));
			}

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
		/// Filters this collection in a way where successive duplicates are removed.
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <param name="collection">The source collection.</param>
		public static IEnumerable<T> RemoveSuccessiveDuplicates<T>(this IEnumerable<T> collection)
		{
			if(collection == null) {
				throw new ArgumentNullException(nameof(collection));
			}

			T previous = default;
			bool isFirst = true;
			IEnumerator<T> enumerator = collection.GetEnumerator();
			while(enumerator.MoveNext()) {
				if(isFirst) {
					isFirst = false;
				} else {
					if(previous.Equals(enumerator.Current)) {
						previous = enumerator.Current;
						continue;
					}
				}
				previous = enumerator.Current;
				yield return enumerator.Current;
			}
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
		/// Projects each element of a sequence into a new form and returns a sequence of only those forms that occurs the most (using a default equality comparer to compare values).
		/// <para>Null (or default) values are also counted and considered as equal.</para>
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
		/// <param name="source">A sequence of values to invoke a transform function on.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		public static IEnumerable<TResult> SelectWhereMaxOccurrenceOf<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
		{
			if(source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if(selector == null) {
				throw new ArgumentNullException(nameof(selector));
			}

			var counts = new Dictionary<TResult, int>();

			int defaultCounter = 0;

			foreach(T element in source) {
				TResult value = selector(element);
				if(value.IsDefault()) {
					++defaultCounter;
				} else if(counts.ContainsKey(value)) {
					++counts[value];
				} else {
					counts.Add(value, 1);
				}
			}

			int max = counts.Count == 0 ? 0 : counts.Max(kvp => kvp.Value);
			if(defaultCounter > max) {
				return new List<TResult> { default };
			}

			if(max == 0) {
				// is empty
				// calling this will also return an empty collection :)
				return source.Select(selector);
			}

			IEnumerable<TResult> result = counts
				.Where(kvp => kvp.Value == max)
				.Select(kvp => kvp.Key);

			if(defaultCounter == max) {
				return new List<TResult>(result) { default };
			}

			return result;
		}

		/// <summary>
		/// Projects each element of a sequence into a new form and returns a sequence of only those forms that occurs the least (using a default equality comparer to compare values).
		/// <para>Null (or default) values are also counted and considered as equal.</para>
		/// </summary>
		/// <typeparam name="T">The type of the elements of source.</typeparam>
		/// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
		/// <param name="source">A sequence of values to invoke a transform function on.</param>
		/// <param name="selector">A transform function to apply to each element.</param>
		public static IEnumerable<TResult> SelectWhereMinOccurrenceOf<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
		{
			if(source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if(selector == null) {
				throw new ArgumentNullException(nameof(selector));
			}

			var counts = new Dictionary<TResult, int>();

			int defaultCounter = 0;

			foreach(T element in source) {
				TResult value = selector(element);
				if(value.IsDefault()) {
					++defaultCounter;
				} else if(counts.ContainsKey(value)) {
					++counts[value];
				} else {
					counts.Add(value, 1);
				}
			}

			int? min = counts.Count == 0 ? null : (int?)counts.Min(kvp => kvp.Value);
			if(defaultCounter > 0 && (min == null || defaultCounter < min)) {
				return new List<TResult> { default };
			}

			if(min == null) {
				// is empty
				// calling this will also return an empty collection :)
				return source.Select(selector);
			}

			IEnumerable<TResult> result = counts
				.Where(kvp => kvp.Value == min.Value)
				.Select(kvp => kvp.Key);

			if(defaultCounter == min.Value) {
				return new List<TResult>(result) { default };
			}

			return result;
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

		/// <summary>
		/// Creates a <see cref="Dictionary{TKey, TValue}"/> from a grouping collection according to the specified element selector function.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TElement">The type of the original elements.</typeparam>
		/// <typeparam name="TSelectedElement">The type of the selected elements.</typeparam>
		/// <param name="groupingCollection">An <see cref="IEnumerable{T}"/> of <see cref="IGrouping{TKey, TElement}"/> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
		/// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
		public static Dictionary<TKey, List<TSelectedElement>> ToDictionaryFromGrouping<TKey, TElement, TSelectedElement>(this IEnumerable<IGrouping<TKey, TElement>> groupingCollection, Func<TElement, TSelectedElement> elementSelector)
		{
			if(groupingCollection == null) {
				throw new ArgumentNullException(nameof(groupingCollection));
			}
			return groupingCollection.ToDictionary(g => g.Key, g => g.Select(elementSelector).ToList());
		}

		/// <summary>
		/// Creates a <see cref="Dictionary{TKey, TValue}"/> from a grouping collection according to the specified key selector and element selector functions.
		/// </summary>
		/// <typeparam name="TKey">The type of the original key.</typeparam>
		/// <typeparam name="TElement">The type of the original elements.</typeparam>
		/// <typeparam name="TSelectedKey">The type of the selected key.</typeparam>
		/// <typeparam name="TSelectedElement">The type of the selected elements.</typeparam>
		/// <param name="groupingCollection">An <see cref="IEnumerable{T}"/> of <see cref="IGrouping{TKey, TElement}"/> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
		/// <param name="keySelector">A function to transform a key.</param>
		/// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
		public static Dictionary<TSelectedKey, List<TSelectedElement>> ToDictionaryFromGrouping<TKey, TElement, TSelectedKey, TSelectedElement>(this IEnumerable<IGrouping<TKey, TElement>> groupingCollection, Func<TKey, TSelectedKey> keySelector, Func<TElement, TSelectedElement> elementSelector)
		{
			if(groupingCollection == null) {
				throw new ArgumentNullException(nameof(groupingCollection));
			}
			if(keySelector == null) {
				throw new ArgumentNullException(nameof(keySelector));
			}
			return groupingCollection.ToDictionary(g => keySelector(g.Key), g => g.Select(elementSelector).ToList());
		}

		/// <summary>
		/// Creates a <see cref="ObservableCollection{T}"/> that contains elements copied from the specified collection.
		/// </summary>
		/// <typeparam name="TElement">Type of the elements.</typeparam>
		/// <param name="collection">The collection from which the elements are copied.</param>
		public static ObservableCollection<TElement> ToObservableCollection<TElement>(this IEnumerable<TElement> collection)
		{
			return new ObservableCollection<TElement>(collection);
		}

		/// <summary>
		/// Returns a sequence of only those elements where the provided transform function returns a max value in the provided collection.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the collection.</typeparam>
		/// <param name="collection">The source collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element that returns the value to compare items with.</param>
		public static List<T> WhereMax<T>(this IEnumerable<T> collection, Func<T, int> valueSelector)
		{
			var list = new List<T>();
			int max = int.MinValue;
			foreach(T item in collection) {
				int current = valueSelector(item);
				if(current == max) {
					list.Add(item);
				} else if(current > max) {
					list.Clear();
					list.Add(item);
					max = current;
				}
			}
			return list;
		}

		/// <summary>
		/// Returns a sequence of only those elements where the provided transform function returns a min value in the provided collection.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the collection.</typeparam>
		/// <param name="collection">The source collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element that returns the value to compare items with.</param>
		public static List<T> WhereMin<T>(this IEnumerable<T> collection, Func<T, int> valueSelector)
		{
			var list = new List<T>();
			int min = int.MaxValue;
			foreach(T item in collection) {
				int current = valueSelector(item);
				if(current == min) {
					list.Add(item);
				} else if(current < min) {
					list.Clear();
					list.Add(item);
					min = current;
				}
			}
			return list;
		}
	}
}
