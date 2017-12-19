/*
MIT License

Copyright (c) 2017 Grega Mohorko

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
Created: 2017-10-29
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Cryptography utilities.
	/// </summary>
	public static class CryptographyUtility
	{
		/// <summary>
		/// Computes and returns the SHA256 hash value of the specified file.
		/// </summary>
		/// <param name="file">The file of which to compute the checksum.</param>
		public static string GetChecksum(string file)
		{
			byte[] checksumBytes;
			using(var sha256=new SHA256Managed()) {
				using(FileStream stream = File.OpenRead(file)) {
					checksumBytes = sha256.ComputeHash(stream);
				}
			}

			string checksum = BitConverter.ToString(checksumBytes);

			// remove the hyphen
			return checksum.RemoveAllOf("-");
		}
	}
}
