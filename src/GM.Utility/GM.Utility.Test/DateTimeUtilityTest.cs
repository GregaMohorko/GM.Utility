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
Created: 2018-9-12
Author: GregaMohorko
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class DateTimeUtilityTest
	{
		[TestMethod]
		public void EndOfMonth()
		{
			DateTime date;
			DateTime endOfMonth;

			// normal 30 day month
			date = new DateTime(2018, 9, 15);
			endOfMonth = date.EndOfMonth();
			Assert.AreEqual(new DateTime(2018, 9, 15), date); // make sure that the original DateTime is not modified
			Assert.AreEqual(new DateTime(2018,9,30), endOfMonth);

			// normal 31 day month
			date = new DateTime(2018, 12, 20);
			endOfMonth = date.EndOfMonth();
			Assert.AreEqual(new DateTime(2018, 12, 31), endOfMonth);

			// february 28
			date = new DateTime(2018, 2, 10);
			endOfMonth = date.EndOfMonth();
			Assert.AreEqual(new DateTime(2018, 2, 28), endOfMonth);

			// february 29
			date = new DateTime(2016, 2, 1);
			endOfMonth = date.EndOfMonth();
			Assert.AreEqual(new DateTime(2016, 2, 29), endOfMonth);

			// already last day
			date = new DateTime(1993, 1, 31);
			endOfMonth = date.EndOfMonth();
			Assert.AreEqual(new DateTime(1993, 1, 31), endOfMonth);

			// current end of month
			date = DateTime.Today.EndOfMonth();
			endOfMonth = DateTimeUtility.EndOfMonth();
			Assert.AreEqual(date, endOfMonth);
		}

		[TestMethod]
		public void StartOfMonth()
		{
			DateTime date;
			DateTime startOfMonth;

			// normal month
			date = new DateTime(2018, 9, 15);
			startOfMonth = date.StartOfMonth();
			Assert.AreEqual(new DateTime(2018, 9, 15), date); // make sure that the original DateTime is not modified
			Assert.AreEqual(new DateTime(2018, 9, 1), startOfMonth);

			// already first day
			date = new DateTime(2000, 6, 1);
			startOfMonth = date.StartOfMonth();
			Assert.AreEqual(new DateTime(2000, 6, 1), startOfMonth);

			// current start of month
			date = DateTime.Today.StartOfMonth();
			startOfMonth = DateTimeUtility.StartOfMonth();
			Assert.AreEqual(date, startOfMonth);
		}
	}
}
