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

public class StringUtilityTests
{
	[Fact]
	public void ShortenWith3Dots()
	{
		Assert.Throws<ArgumentNullException>(() => StringUtility.ShortenWith3Dots(null, -42));
		Assert.Throws<ArgumentOutOfRangeException>(() => StringUtility.ShortenWith3Dots("", -1));
		Assert.Throws<ArgumentOutOfRangeException>(() => StringUtility.ShortenWith3Dots("", -100));
		Assert.Throws<ArgumentOutOfRangeException>(() => StringUtility.ShortenWith3Dots("", int.MinValue));
		Assert.Throws<ArgumentOutOfRangeException>(() => StringUtility.ShortenWith3Dots("", 0));

		string text = "Blues for the Red Sun";
		Assert.Equal(text, text.ShortenWith3Dots(int.MaxValue));
		Assert.Equal(text, text.ShortenWith3Dots(text.Length));
		Assert.Equal("B...", text.ShortenWith3Dots(1));
		Assert.Equal("Blues...", text.ShortenWith3Dots(5));
		Assert.Equal("Blues for the Red Su...", text.ShortenWith3Dots(20));
	}
}
