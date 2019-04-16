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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for Dictionary.
	/// </summary>
	public static class DictionaryUtility
	{
		/// <summary>
		/// Returns a read-only <see cref="ReadOnlyDictionary{TKey, TValue}"/> wrapper for the current dictionary.
		/// </summary>
		/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			return new ReadOnlyDictionary<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Creates two dictionaries: singles, that contains objects that are the only ones with a unique key, and lists, that contains lists of objects with the same key. Key is selected with the provided key selecting function.
		/// </summary>
		/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
		/// <param name="objects">List of objects which to sort into the two dictionaries.</param>
		/// <param name="singles">Dictionary with objects that have unique keys.</param>
		/// <param name="lists">Dictionary with lists of objects that have the same key.</param>
		/// <param name="keySelector">Function with which to select the key from an object.</param>
		public static void CreateKeyDictionaries<TKey, TValue>(List<TValue> objects, out Dictionary<TKey, TValue> singles, out Dictionary<TKey, List<TValue>> lists, Func<TValue, TKey> keySelector)
		{
			singles = new Dictionary<TKey, TValue>();
			lists = new Dictionary<TKey, List<TValue>>();

			IEnumerable<IGrouping<TKey, TValue>> groups = objects.GroupBy(keySelector);

			foreach(IGrouping<TKey, TValue> group in groups) {
				List<TValue> groupObjects = group.ToList();

				if(groupObjects.Count > 1) {
					lists.Add(group.Key, groupObjects);
				} else {
					singles.Add(group.Key, groupObjects[0]);
				}
			}
		}

		/// <summary>
		/// Gets the value associated with the specified key. If no entry with the specified key exists, it is created.
		/// </summary>
		/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key whose value to get or create.</param>
		public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
		{
			if(dictionary == null) {
				throw new ArgumentNullException(nameof(dictionary));
			}
			if(!dictionary.TryGetValue(key, out TValue value)) {
				value = new TValue();
				dictionary.Add(key, value);
			}
			return value;
		}
	}
}
