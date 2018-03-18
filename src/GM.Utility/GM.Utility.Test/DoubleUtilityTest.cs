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
Created: 2018-3-18
Author: GregaMohorko
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class DoubleUtilityTest
	{
		[TestMethod]
		public void GetDecimals()
		{
			Assert.AreEqual(0.123456789, 42.123456789.GetDecimals());
			Assert.AreEqual(-0.123456789, -42.123456789.GetDecimals());
			Assert.AreEqual(0.123456789, 0.123456789.GetDecimals());
			Assert.AreEqual(0.987654321, 1.987654321.GetDecimals());
			Assert.AreEqual(0d, 123d.GetDecimals());
		}

		[TestMethod]
		public void GetDecimalPart()
		{
			Assert.AreEqual(1, 42.123.GetDecimalPart(1));
			Assert.AreEqual(12, 42.123.GetDecimalPart(2));
			Assert.AreEqual(123, 42.123.GetDecimalPart(3));
			Assert.AreEqual(123456789, 42.123456789.GetDecimalPart(9));
			Assert.AreEqual(1234567, 42.123456789.GetDecimalPart(7));
			Assert.AreEqual(-98, -42.987.GetDecimalPart(2));
			Assert.AreEqual(0, 42d.GetDecimalPart(1));
			Assert.AreEqual(0, 1.23.GetDecimalPart(0));
		}

		[TestMethod]
		public void GetDecimalPart_Whole()
		{
			Assert.AreEqual(1, 42.1.GetDecimalPart());
			Assert.AreEqual(12, 0.12.GetDecimalPart());
			Assert.AreEqual(-123, -456.123.GetDecimalPart());
			Assert.AreEqual(123456789, 42.123456789.GetDecimalPart());
			Assert.AreEqual(0, 123d.GetDecimalPart());
		}
	}
}
