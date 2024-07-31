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
Created: 2017-10-29
Author: Gregor Mohorko
*/

using System;
using System.IO;
using System.Security.Cryptography;

namespace GM.Utility;

/// <summary>
/// Cryptography utilities.
/// </summary>
public static class CryptographyUtility
{
	/// <summary>
	/// Gets a new cryptographically strong random salt.
	/// <para>
	/// The salt is a sequence of bytes that is added to the password before being digested. This makes our digests different to what they would be if we encrypted the password alone, and as a result protects us against dictionary attacks.
	/// </para>
	/// </summary>
	/// <returns>A new random salt of type long (8 bytes).</returns>
	public static long CreateRandomSalt()
	{
		byte[] saltBytes = new byte[8];

		// Gets cryptographically strong random bytes
		using(var rng = new RNGCryptoServiceProvider()) {
			rng.GetBytes(saltBytes);
		}

		// Bytes -> Long
		return BitConverter.ToInt64(saltBytes, 0);
	}

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
