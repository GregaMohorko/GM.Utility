/*
MIT License

Copyright (c) 2018 Grega Mohorko

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
Created: 2018-4-16
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="Random"/>.
	/// </summary>
    public static class RandomUtility
	{
		/// <summary>
		/// Creates and returns an array of the specified size containing random integer values within the [low, high] interval.
		/// </summary>
		/// <param name="rand">The <see cref="Random"/> object to use to create random values.</param>
		/// <param name="minValue">The inclusive lower bound of the random numbers.</param>
		/// <param name="maxValue">The exclusive upper bound of the random numbers. maxValue must be greater than or equal to minValue.</param>
		/// <param name="size">The size of the array to create.</param>
		public static int[] NextIntegers(this Random rand, int minValue, int maxValue, int size)
		{
			if(rand == null) {
				throw new ArgumentNullException(nameof(rand));
			}

			var result = new int[size];
			for(int i = 0; i < size; ++i) {
				result[i] = rand.Next(minValue, maxValue);
			}
			return result;
		}

		/// <summary>
		/// Creates and returns an array of the specified size containing random integer values within the [low, high] interval.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random numbers.</param>
		/// <param name="maxValue">The exclusive upper bound of the random numbers. maxValue must be greater than or equal to minValue.</param>
		/// <param name="size">The size of the array to create.</param>
		public static int[] RandomIntegers(int minValue, int maxValue, int size)
		{
			var rand = new Random();
			return NextIntegers(rand, minValue, maxValue, size);
		}

		/// <summary>
		/// Creates and returns an array of the specified size containing random values within the [low, high] interval.
		/// <para>This method is deprecated, please use <see cref="RandomIntegers(int, int, int)"/> instead.</para>
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random numbers.</param>
		/// <param name="maxValue">The exclusive upper bound of the random numbers. maxValue must be greater than or equal to minValue.</param>
		/// <param name="size">The size of the array to create.</param>
		[Obsolete("Method RandomInts is deprecated, please use RandomIntegers instead.",true)]
		public static int[] RandomInts(int minValue, int maxValue, int size)
		{
			// FIXME obsolete 2018-4-16
			return RandomIntegers(minValue, maxValue, size);
		}

		/// <summary>
		/// Creates and returns an array of the specified size containing random values within the [low, high] interval.
		/// <para>
		/// This method is deprecated, please use <see cref="NextIntegers(Random, int, int, int)"/> instead.
		/// </para>
		/// </summary>
		/// <param name="rand">The <see cref="Random"/> object to use to create random values.</param>
		/// <param name="minValue">The inclusive lower bound of the random numbers.</param>
		/// <param name="maxValue">The exclusive upper bound of the random numbers. maxValue must be greater than or equal to minValue.</param>
		/// <param name="size">The size of the array to create.</param>
		[Obsolete("Method RandomInts is deprecated, please use NextIntegers instead.",true)]
		public static int[] RandomInts(this Random rand, int minValue, int maxValue, int size)
		{
			// FIXME obsolete 2018-4-16
			return NextIntegers(rand, minValue, maxValue, size);
		}
	}
}
