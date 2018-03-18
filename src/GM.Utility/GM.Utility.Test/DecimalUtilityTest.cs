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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class DecimalUtilityTest
	{
		[TestMethod]
		public void GetDecimals()
		{
			Assert.AreEqual(0.123456789m, 42.123456789m.GetDecimals());
			Assert.AreEqual(-0.123456789m, -42.123456789m.GetDecimals());
			Assert.AreEqual(0.123456789m, 0.123456789m.GetDecimals());
			Assert.AreEqual(0.987654321m, 1.987654321m.GetDecimals());
			Assert.AreEqual(0m, 123m.GetDecimals());
		}

		[TestMethod]
		public void GetDecimalPart()
		{
			Assert.AreEqual(1, 42.123m.GetDecimalPart(1));
			Assert.AreEqual(12, 42.123m.GetDecimalPart(2));
			Assert.AreEqual(123, 42.123m.GetDecimalPart(3));
			Assert.AreEqual(123456789, 42.123456789m.GetDecimalPart(9));
			Assert.AreEqual(1234567, 42.123456789m.GetDecimalPart(7));
			Assert.AreEqual(-98, -42.987m.GetDecimalPart(2));
			Assert.AreEqual(0, 42m.GetDecimalPart(1));
			Assert.AreEqual(0, 1.23m.GetDecimalPart(0));
		}

		[TestMethod]
		public void GetDecimalPart_Whole()
		{
			Assert.AreEqual(1, 42.1m.GetDecimalPart());
			Assert.AreEqual(12, 0.12m.GetDecimalPart());
			Assert.AreEqual(-123, -456.123m.GetDecimalPart());
			Assert.AreEqual(123456789, 42.123456789m.GetDecimalPart());
			Assert.AreEqual(0, 123m.GetDecimalPart());
		}
	}
}
