/*
MIT License

Copyright (c) 2023 Gregor Mohorko

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
Created: 2018-2-25
Author: GregaMohorko
*/

using System;

namespace GM.Utility;

/// <summary>
/// Utilities for <see cref="DayOfWeek"/>.
/// </summary>
public static class DayOfWeekUtility
{
	/// <summary>
	/// Gets the full name of this week day based on the current UI culture.
	/// </summary>
	/// <param name="dayOfWeek">The week day for which to get the name.</param>
	public static string GetName(this DayOfWeek dayOfWeek)
	{
		return GlobalizationUtility.GetWeekDayName(dayOfWeek);
	}

	/// <summary>
	/// Determines whether this day of week is the weekend.
	/// <para>Assumes that Saturday and Sunday are weekend days.</para>
	/// </summary>
	/// <param name="dayOfWeek">The day of week.</param>
	public static bool IsWeekend(this DayOfWeek dayOfWeek)
	{
		return dayOfWeek == DayOfWeek.Saturday
			|| dayOfWeek == DayOfWeek.Sunday;
	}
}
