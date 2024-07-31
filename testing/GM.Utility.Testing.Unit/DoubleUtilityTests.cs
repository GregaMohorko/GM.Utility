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

public class DoubleUtilityTests
{
	[Fact]
	public void GetDecimals()
	{
		Assert.Equal(0.123456789, 42.123456789.GetDecimals());
		Assert.Equal(-0.123456789, -42.123456789.GetDecimals());
		Assert.Equal(0.123456789, 0.123456789.GetDecimals());
		Assert.Equal(0.987654321, 1.987654321.GetDecimals());
		Assert.Equal(0d, 123d.GetDecimals());
	}

	[Fact]
	public void GetDecimals_WithRounding()
	{
		Assert.Equal(0.123, 42.123456789.GetDecimals(3));
		Assert.Equal(-0.1235, -42.123456789.GetDecimals(4));
		Assert.Equal(0.12346, 0.123456789.GetDecimals(5));
		Assert.Equal(0.987654, 1.987654321.GetDecimals(6));
		Assert.Equal(0d, 123d.GetDecimals(7));
	}

	[Fact]
	public void GetDecimalPart()
	{
		Assert.Equal(1, 42.123.GetDecimalPart(1, false));
		Assert.Equal(1, 42.123.GetDecimalPart(1, true));
		Assert.Equal(7, 42.789.GetDecimalPart(1, false));
		Assert.Equal(8, 42.789.GetDecimalPart(1, true));
		Assert.Equal(12, 42.123.GetDecimalPart(2, false));
		Assert.Equal(12, 42.123.GetDecimalPart(2, true));
		Assert.Equal(123, 42.123.GetDecimalPart(3, false));
		Assert.Equal(123, 42.123.GetDecimalPart(3, true));
		Assert.Equal(123456789, 42.123456789.GetDecimalPart(9, false));
		Assert.Equal(123456789, 42.123456789.GetDecimalPart(9, true));
		Assert.Equal(1234567, 42.123456789.GetDecimalPart(7, false));
		Assert.Equal(1234568, 42.123456789.GetDecimalPart(7, true));
		Assert.Equal(-98, -42.987.GetDecimalPart(2, false));
		Assert.Equal(-99, -42.987.GetDecimalPart(2, true));
		Assert.Equal(0, 42d.GetDecimalPart(1, false));
		Assert.Equal(0, 42d.GetDecimalPart(1, true));
		Assert.Equal(0, 1.23.GetDecimalPart(0, false));
		Assert.Equal(0, 1.23.GetDecimalPart(0, true));
	}

	[Fact]
	public void GetDecimalPart_Whole()
	{
		Assert.Equal(1, 42.1.GetDecimalPart());
		Assert.Equal(12, 0.12.GetDecimalPart());
		Assert.Equal(-123, -456.123.GetDecimalPart());
		Assert.Equal(123456789, 42.123456789.GetDecimalPart());
		Assert.Equal(0, 123d.GetDecimalPart());
	}
}
