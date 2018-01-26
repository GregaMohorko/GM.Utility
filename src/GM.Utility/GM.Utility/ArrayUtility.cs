﻿/*
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
	/// Utilities for arrays.
	/// </summary>
	public static class ArrayUtility
	{
		/// <summary>
		/// Adds the provided item to the end of the array.
		/// </summary>
		/// <typeparam name="T">Type of the items in the array.</typeparam>
		/// <param name="array">The array to add the item to.</param>
		/// <param name="item">The item to add to the array.</param>
		public static T[] Add<T>(this T[] array, T item)
		{
			Array.Resize(ref array, array.Length + 1);
			array[array.Length - 1] = item;
			return array;
		}

		/// <summary>
		/// Performs the specified action for each element in the array. Supports multiple dimensions, the second parameter of the action are current indices for the dimensions.
		/// <para>
		/// To use the indices, you can use the <see cref="Array.GetValue(int[])"/> and <see cref="Array.SetValue(object, int[])"/>.
		/// </para>
		/// </summary>
		/// <param name="array">The array that contains the elements.</param>
		/// <param name="action">An action to invoke for each element. The second parameter is an int array that represents the indexes of the current element for each dimension.</param>
		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if(array.LongLength == 0) {
				return;
			}

			ArrayTraverse walker = new ArrayTraverse(array);

			do {
				action(array, walker.Position);
			} while(walker.Step());
		}

		/// <summary>
		/// Swaps two indexes in the array.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the array.</typeparam>
		/// <param name="array">The array in which to swap two elements.</param>
		/// <param name="index1">The index of the first element.</param>
		/// <param name="index2">The index of the second element.</param>
		public static void Swap<T>(this T[] array, int index1, int index2)
		{
			Util.Swap(ref array[index1], ref array[index2]);
		}

		/// <summary>
		/// Swaps two rows in the array.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the array.</typeparam>
		/// <param name="array">The array in which to swap two rows.</param>
		/// <param name="row1">First row.</param>
		/// <param name="row2">Second row.</param>
		public static void SwapRow<T>(this T[,] array, int row1, int row2)
		{
			for(int i = array.GetLength(1) - 1; i >= 0; --i) {
				Util.Swap(ref array[row1, i], ref array[row2, i]);
			}
		}

		private class ArrayTraverse
		{
			public int[] Position;
			private int[] maxLengths;

			public ArrayTraverse(Array array)
			{
				Position = new int[array.Rank];

				maxLengths = new int[array.Rank];
				for(int i = 0; i < array.Rank; i++) {
					maxLengths[i] = array.GetLength(i) - 1;
				}
			}

			public bool Step()
			{
				for(int i = 0; i < Position.Length; i++)
					if(Position[i] < maxLengths[i]) {
						Position[i]++;
						for(int j = 0; j < i; j++) {
							Position[j] = 0;
						}

						return true;
					}

				return false;
			}
		}
	}
}