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
Created: 2017-10-27
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// General utilities.
	/// </summary>
	public static class Util
	{
		/// <summary>
		/// Returns combined array with the mandatory parameter in the 1st index, following with params array.
		/// </summary>
		/// <typeparam name="T">The type of elements.</typeparam>
		/// <param name="mandatoryParameter">The mandatory parameter. Must not be default.</param>
		/// <param name="paramsArray">The params array.</param>
		/// <exception cref="ArgumentNullException">Thrown when the <paramref name="mandatoryParameter"/> is default.</exception>
		public static T[] CombineWithParams<T>(T mandatoryParameter, T[] paramsArray)
		{
			if(Equals(mandatoryParameter, default(T))) {
				throw new ArgumentNullException(nameof(mandatoryParameter));
			}

			if(paramsArray.IsNullOrEmpty()) {
				return new T[1] { mandatoryParameter };
			}

			// combine
			var combinedArray = new T[paramsArray.Length + 1];
			combinedArray[0] = mandatoryParameter;
			Array.Copy(paramsArray, 0, combinedArray, 1, paramsArray.Length);
			return combinedArray;
		}

		/// <summary>
		/// Returns the item for which the provided transform function returns the max value of all the provided items. If multiple items have max value, the first is returned.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="valueSelector">A transform function to apply to each item that returns the value to compare items with.</param>
		/// <param name="items">The items.</param>
		public static T SelectOneWithMax<T>(Func<T, double> valueSelector, params T[] items)
		{
			if(items == null) {
				throw new ArgumentNullException(nameof(items));
			}
			if(items.Length == 0) {
				throw new ArgumentOutOfRangeException(nameof(items), "At least one item must be provided.");
			}
			if(items.Length == 1) {
				return items[0];
			}

			T itemWithMax = items[0];
			double max = valueSelector(items[0]);
			for(int i = 1; i < items.Length; ++i) {
				T item = items[i];
				double current = valueSelector(item);
				if(current > max) {
					max = current;
					itemWithMax = item;
				}
			}
			return itemWithMax;
		}

		/// <summary>
		/// Returns all items for which the provided transform function returns the max value of all the provided items.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="valueSelector">A transform function to apply to each item that returns the value to compare items with.</param>
		/// <param name="items">The items.</param>
		public static List<T> SelectThoseWithMax<T>(Func<T, double> valueSelector, params T[] items)
		{
			if(items == null) {
				throw new ArgumentNullException(nameof(items));
			}
			if(items.Length == 0) {
				throw new ArgumentOutOfRangeException(nameof(items), "At least one item must be provided.");
			}
			if(items.Length == 1) {
				return new List<T> { items[0] };
			}

			var itemsWithMax = new List<T> { items[0] };
			double max = valueSelector(items[0]);
			for(int i = 1; i < items.Length; ++i) {
				T item = items[i];
				double current = valueSelector(item);
				if(current > max) {
					max = current;
					itemsWithMax.Clear();
					itemsWithMax.Add(item);
				} else if(current == max) {
					itemsWithMax.Add(item);
				}
			}
			return itemsWithMax;
		}

		/// <summary>
		/// Swaps the values in both provided variables.
		/// </summary>
		/// <typeparam name="T">The type of the variables.</typeparam>
		/// <param name="A">First variable.</param>
		/// <param name="B">Second variable.</param>
		public static void Swap<T>(ref T A, ref T B)
		{
			T swap = A;
			A = B;
			B = swap;
		}
	}
}
