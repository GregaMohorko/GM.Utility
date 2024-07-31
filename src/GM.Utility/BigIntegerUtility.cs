/*
MIT License

Copyright (c) 2022 Gregor Mohorko

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
Created: 2022-11-2
Author: Gregor Mohorko
*/

using System;
using System.Numerics;

namespace GM.Utility;
/// <summary>
/// Utilities for <see cref="BigInteger"/>.
/// </summary>
public static class BigIntegerUtility
{
	/// <summary>
	/// Changes the last N digits to zero. Useful for rounding <br/>
	/// e.g.:<br/>
	/// 12_345_678, 0 -> 12_345_678 <br/>
	/// 12_345_678, 1 -> 12_345_670 <br/>
	/// 12_345_678, 2 -> 12_345_600 <br/>
	/// 12_345_678, 3 -> 12_345_000 <br/>
	/// </summary>
	/// <param name="value">Number to change last digits to</param>
	/// <param name="n">number of digits to change to 0</param>
	/// <exception cref="ArgumentOutOfRangeException">n is a negative number</exception>
	public static BigInteger ChangeLastNDigitsToZero(this BigInteger value, int n)
	{
		if(n < 0) {
			throw new ArgumentOutOfRangeException(nameof(n), n, "n can't be negative");
		}
		var denomination = BigInteger.Pow(10, n);
		return BigInteger.Multiply(BigInteger.Divide(value, denomination), denomination);
	}
}
