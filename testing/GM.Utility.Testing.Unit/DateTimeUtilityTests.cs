/*
MIT License

Copyright (c) 2024 Gregor Mohorko

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

Project: GM.Utility.Testing.Unit
Created: 2024-7-31
Author: grega
*/

namespace GM.Utility.Testing.Unit;

public class DateTimeUtilityTests
{
	[Fact]
	public void DurationBetween()
	{
		const int MS_IN_ONE_DAY = 24 * 60 * 60 * 1000;

		DateTime date1;
		DateTime date2;
		TimeSpan durationBetween;

		// ascending order
		date1 = new DateTime(2021, 12, 17);
		date2 = new DateTime(2021, 12, 18);
		durationBetween = DateTimeUtility.DurationBetween(date1, date2);
		durationBetween.Days.Should().Be(1);
		durationBetween.TotalMilliseconds.Should().Be(MS_IN_ONE_DAY);

		// descending order
		date1 = new DateTime(2021, 12, 18);
		date2 = new DateTime(2021, 12, 17);
		durationBetween = DateTimeUtility.DurationBetween(date1, date2);
		durationBetween.Days.Should().Be(1);
		durationBetween.TotalMilliseconds.Should().Be(MS_IN_ONE_DAY);
	}

	[Fact]
	public void EndOfMonth()
	{
		DateTime date;
		DateTime endOfMonth;

		// normal 30 day month
		date = new DateTime(2018, 9, 15);
		endOfMonth = date.EndOfMonth();
		date.Should().Be(new DateTime(2018, 9, 15)); // make sure that the original DateTime is not modified
		endOfMonth.Should().Be(new DateTime(2018, 9, 30));

		// normal 31 day month
		date = new DateTime(2018, 12, 20);
		endOfMonth = date.EndOfMonth();
		endOfMonth.Should().Be(new DateTime(2018, 12, 31));

		// february 28
		date = new DateTime(2018, 2, 10);
		endOfMonth = date.EndOfMonth();
		endOfMonth.Should().Be(new DateTime(2018, 2, 28));

		// february 29
		date = new DateTime(2016, 2, 1);
		endOfMonth = date.EndOfMonth();
		endOfMonth.Should().Be(new DateTime(2016, 2, 29));

		// already last day
		date = new DateTime(1993, 1, 31);
		endOfMonth = date.EndOfMonth();
		endOfMonth.Should().Be(new DateTime(1993, 1, 31));

		// current end of month
		date = DateTime.Today.EndOfMonth();
		endOfMonth = DateTimeUtility.EndOfMonth();
		endOfMonth.Should().Be(date);
	}

	[Fact]
	public void StartOfMonth()
	{
		DateTime date;
		DateTime startOfMonth;

		// normal month
		date = new DateTime(2018, 9, 15);
		startOfMonth = date.StartOfMonth();
		date.Should().Be(new DateTime(2018, 9, 15)); // make sure that the original DateTime is not modified
		startOfMonth.Should().Be(new DateTime(2018, 9, 1));

		// already first day
		date = new DateTime(2000, 6, 1);
		startOfMonth = date.StartOfMonth();
		startOfMonth.Should().Be(new DateTime(2000, 6, 1));

		// current start of month
		date = DateTime.Today.StartOfMonth();
		startOfMonth = DateTimeUtility.StartOfMonth();
		startOfMonth.Should().Be(date);
	}
}
