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

Project: GM.Utility.Test
Created: 2018-4-16
Author: GregaMohorko
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class ArrayUtilityTest
	{
		[TestMethod]
		public void Reset()
		{
			Assert.ThrowsException<ArgumentNullException>(delegate
			{
				ArrayUtility.Reset<string>(null);
			});
			Assert.ThrowsException<ArgumentNullException>(delegate
			{
				ArrayUtility.Reset(null,"");
			});

			var array1 = new int[] { 1,2,3,4,5,6,7,8,9,10 };
			array1.Reset();
			var expected1 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			CollectionAssert.AreEqual(expected1, array1);

			var array2 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
			array2.Reset(42);
			var expected2 = new int[] { 42, 42, 42, 42, 42, 42, 42, 42, 42, 42 };
			CollectionAssert.AreEqual(expected2, array2);
		}
	}
}
