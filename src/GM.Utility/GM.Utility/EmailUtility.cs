/*
MIT License

Copyright (c) 2019 Grega Mohorko

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
Created: 2019-1-28
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for email.
	/// </summary>
	public static class EmailUtility
	{
		/// <summary>
		/// Determines whether the specified email address matches the pattern of a valid email address.
		/// </summary>
		/// <param name="emailAddress">The address to validate.</param>
		public static bool IsValid(string emailAddress)
		{
			try {
				var addr = new MailAddress(emailAddress);
				return addr.Address == emailAddress;
			} catch {
				return false;
			}
		}
	}
}
