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

public class DecimalUtilityTests
{
	[Fact]
	public void GetDecimals()
	{
		42.123456789m.GetDecimals().Should().Be(0.123456789m);
		(-42.123456789m).GetDecimals().Should().Be(-0.123456789m);
		0.123456789m.GetDecimals().Should().Be(0.123456789m);
		1.987654321m.GetDecimals().Should().Be(0.987654321m);
		123m.GetDecimals().Should().Be(0m);
	}

	[Fact]
	public void GetDecimals_WithRounding()
	{
		42.123456789m.GetDecimals(3).Should().Be(0.123m);
		(-42.123456789m).GetDecimals(4).Should().Be(-0.1235m);
		0.123456789m.GetDecimals(5).Should().Be(0.12346m);
		1.987654321m.GetDecimals(6).Should().Be(0.987654m);
		123m.GetDecimals(7).Should().Be(0m);
	}

	[Fact]
	public void GetDecimalPart()
	{
		42.123m.GetDecimalPart(1, false).Should().Be(1);
		42.123m.GetDecimalPart(1, true).Should().Be(1);
		42.789m.GetDecimalPart(1, false).Should().Be(7);
		42.789m.GetDecimalPart(1, true).Should().Be(8);
		42.123m.GetDecimalPart(2, false).Should().Be(12);
		42.123m.GetDecimalPart(2, true).Should().Be(12);
		42.123m.GetDecimalPart(3, false).Should().Be(123);
		42.123m.GetDecimalPart(3, true).Should().Be(123);
		42.123456789m.GetDecimalPart(9, false).Should().Be(123456789);
		42.123456789m.GetDecimalPart(9, true).Should().Be(123456789);
		42.123456789m.GetDecimalPart(7, false).Should().Be(1234567);
		42.123456789m.GetDecimalPart(7, true).Should().Be(1234568);
		(-42.987m).GetDecimalPart(2, false).Should().Be(-98);
		(-42.987m).GetDecimalPart(2, true).Should().Be(-99);
		42m.GetDecimalPart(1, false).Should().Be(0);
		42m.GetDecimalPart(1, true).Should().Be(0);
		1.23m.GetDecimalPart(0, false).Should().Be(0);
		1.23m.GetDecimalPart(0, true).Should().Be(0);
	}

	[Fact]
	public void GetDecimalPart_Whole()
	{
		42.1m.GetDecimalPart().Should().Be(1);
		0.12m.GetDecimalPart().Should().Be(12);
		(-456.123m).GetDecimalPart().Should().Be(-123);
		42.123456789m.GetDecimalPart().Should().Be(123456789);
		123m.GetDecimalPart().Should().Be(0);
	}
}
