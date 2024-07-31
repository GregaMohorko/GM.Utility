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

public class IEnumerableUtilityTests
{
	[Fact]
	public void AllSame1()
	{
		char valueSelector1(string s) => s[0];
		char valueSelector3(string s) => s[2];
		var example = new List<string> { "abc", "aac", "bac" };

		Assert.Throws<ArgumentNullException>(delegate
		{
			IEnumerableUtility.AllSame<string, char>(null, valueSelector1);
		});
		Assert.Throws<ArgumentNullException>(delegate
		{
			IEnumerableUtility.AllSame<string, char>(example, null);
		});

		// all strings have the same character in the third position
		Assert.True(example.AllSame(valueSelector3));
		// not all strings have the same character in the first position
		Assert.False(example.AllSame(valueSelector1));

		// empty collections should return true, the same as LINQ All method
		Assert.True(new List<string>().AllSame(valueSelector1));
	}

	[Fact]
	public void AllSame2()
	{
		char valueSelector1(string s) => s[0];
		char valueSelector2(string s) => s[1];
		char valueSelector3(string s) => s[2];
		char valueSelector4(string s) => s[3];
		var example = new List<string> { "abcd", "aacd", "bacd" };

		Assert.Throws<ArgumentNullException>(delegate
		{
			IEnumerableUtility.AllSame<string, char, char>(null, valueSelector1, valueSelector1);
		});
		Assert.Throws<ArgumentNullException>(delegate
		{
			IEnumerableUtility.AllSame<string, char, char>(example, null, valueSelector1);
		});
		Assert.Throws<ArgumentNullException>(delegate
		{
			IEnumerableUtility.AllSame<string, char, char>(example, valueSelector1, null);
		});

		// all strings have the same character in the third and forth position
		Assert.True(example.AllSame(valueSelector3,valueSelector4));
		// not all strings have the same character in the first and third position
		Assert.False(example.AllSame(valueSelector1, valueSelector3));
		// not all strings have the same character in the third and first position
		Assert.False(example.AllSame(valueSelector3, valueSelector1));
		// not all strings have the same character in the first and second position
		Assert.False(example.AllSame(valueSelector1, valueSelector2));

		// empty collections should return true, the same as LINQ All method
		Assert.True(new List<string>().AllSame(valueSelector1, valueSelector2));
	}

	[Fact]
	public void Rotate()
	{
		var example = new List<int> { 1, 2, 3, 4 };
		List<int> rotated = example.Rotate(1).ToList();
		rotated.Should().Equal(new List<int> { 4, 1, 2, 3 });

		example = new List<int> { 1, 2, 3, 4, 5 };
		rotated = example.Rotate(-2).ToList();
		rotated.Should().Equal(new List<int> { 3, 4, 5, 1, 2 });
	}
}
