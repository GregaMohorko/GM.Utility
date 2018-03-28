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
Created: 2018-3-28
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class IEnumerableUtilityTest
	{
		[TestMethod]
		public void Rotate()
		{
			var example = new List<int> { 1, 2, 3, 4 };
			List<int> rotated = example.Rotate(1).ToList();
			CollectionAssert.AreEqual(new List<int> { 4, 1, 2, 3 }, rotated);

			example = new List<int> { 1, 2, 3, 4, 5 };
			rotated = example.Rotate(-2).ToList();
			CollectionAssert.AreEqual(new List<int> { 3, 4, 5, 1, 2 }, rotated);
		}
	}
}
