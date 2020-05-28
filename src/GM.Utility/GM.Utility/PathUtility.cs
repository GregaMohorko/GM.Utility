/*
MIT License

Copyright (c) 2020 Gregor Mohorko

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
Created: 2018-3-9
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for operations on <see cref="string"/> instances that contain file or directory path information. These operations are performed in a cross-platform manner.
	/// <para>This class offers utilities that can be used with the <see cref="Path"/> class.</para>
	/// </summary>
	public static class PathUtility
	{
		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <exception cref="ArgumentNullException"><paramref name="fromPath"/> or <paramref name="toPath"/> is <c>null</c>.</exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static string GetRelativePath(string fromPath, string toPath)
		{
			// https://stackoverflow.com/a/32113484/6277755

			if(string.IsNullOrEmpty(fromPath)) {
				throw new ArgumentNullException(nameof(fromPath));
			}
			if(string.IsNullOrEmpty(toPath)) {
				throw new ArgumentNullException(nameof(toPath));
			}

			Uri fromUri = new Uri(AppendDirectorySeparatorChar(fromPath));
			Uri toUri = new Uri(AppendDirectorySeparatorChar(toPath));

			if(fromUri.Scheme != toUri.Scheme) {
				throw new InvalidOperationException("The scheme of the provided paths must be equal. For example, a relative path between a FILE and a HTTP cannot be made.");
			}

			Uri relativeUri = fromUri.MakeRelativeUri(toUri);
			string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			if(string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase)) {
				relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}

			return relativePath;
		}

		private static string AppendDirectorySeparatorChar(string path)
		{
			// append a slash only if the path is a directory and does not have a slash at the end
			if(!Path.HasExtension(path) && !path.EndsWith(Path.DirectorySeparatorChar.ToString())) {
				return path + Path.DirectorySeparatorChar;
			}
			return path;
		}

		/// <summary>
		/// Converts the specified name to a valid file name.
		/// <para>Any invalid characters are replaced with '_'.</para>
		/// <para>Spaces are removed.</para>
		/// </summary>
		/// <param name="name">The name to convert.</param>
		public static string GetSafeFileName(string name)
		{
			var buffer = new StringBuilder(name);

			char[] invalidChars = Path.GetInvalidFileNameChars();
			foreach(char c in invalidChars) {
				buffer = buffer.Replace(c, '_');
			}
			buffer[0] = char.ToUpperInvariant(buffer[0]);
			buffer = buffer.Replace(" ", "");

			return buffer.ToString();
		}

		/// <summary>
		/// Converts the specified name to a valid file name (without extension).
		/// <para>Any invalid characters (including dots) are replaced with '_'.</para>
		/// <para>Spaces are removed.</para>
		/// </summary>
		/// <param name="name">The name to convert.</param>
		public static string GetSafeFileNameWithoutExtension(string name)
		{
			return GetSafeFileName(name).Replace('.', '_');
		}

		/// <summary>
		/// Determines whether or not the specified path is a network drive.
		/// <para>All UNC paths are considered as network paths.</para>
		/// </summary>
		/// <param name="path">The path to check if it is a network drive.</param>
		public static bool IsNetworkDrive(string path)
		{
			if(path == null) {
				throw new ArgumentNullException(nameof(path));
			}
			if(path.Length == 0) {
				return false;
			}
			if(path[0] == '/' || path[0] == '\\') {
				return true; // is a UNC path
			}
			// get drive's letter
			string rootPath = Path.GetPathRoot(path);
			var driveInfo = new DriveInfo(rootPath);
			return driveInfo.DriveType == DriveType.Network;
		}

		private static readonly Regex safeFileNameValidIdentifier = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$", RegexOptions.Compiled | RegexOptions.Singleline);

		/// <summary>
		/// Determines whether the provided name is a valid file name.
		/// </summary>
		/// <param name="name">The name to check.</param>
		public static bool IsSafeFileName(string name)
		{
			if(string.IsNullOrWhiteSpace(name)) {
				return false;
			}

			if(!safeFileNameValidIdentifier.IsMatch(name)) {
				return false;
			}

			char[] invalidChars = Path.GetInvalidFileNameChars();
			foreach(char c in name) {
				if(invalidChars.Contains(c)) {
					return false;
				}
			}

			return true;
		}
	}
}
