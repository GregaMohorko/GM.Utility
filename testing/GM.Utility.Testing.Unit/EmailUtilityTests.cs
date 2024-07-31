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

public class EmailUtilityTests
{
	[Fact]
	public void IsValid()
	{
		Assert.True(EmailUtility.IsValid("example@mail.com"));
		Assert.True(EmailUtility.IsValid("ExaMple.EXAMPLE@someDOMAIN.cOm"));
		Assert.True(EmailUtility.IsValid("ALL.CAPS.MULTIPLE.DOTS@DOMAIN.COM"));
		Assert.False(EmailUtility.IsValid("nodot@mailcom"));
		Assert.False(EmailUtility.IsValid("no.dot@mail"));
		Assert.False(EmailUtility.IsValid("no.no.no.no.dot@mydomain"));
	}
}
