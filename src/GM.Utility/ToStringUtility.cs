/*
MIT License

Copyright (c) 2021 Gregor Mohorko

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
Created: 2021-10-27
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for overriding the <see cref="object.ToString"/> method.
	/// </summary>
	public static class ToStringUtility
	{
		/// <summary>
		/// Returns the string representation in the format of "TypeName: Property=Value; Property=Value; ...".
		/// </summary>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <param name="properties">Property names and values.</param>
		public static string ToString<T>(params (string propName, object propValue)[] properties)
		{
			return ToString(typeof(T), properties);
		}

		/// <summary>
		/// Returns the string representation in the format of "TypeName: Property=Value; Property=Value; ...".
		/// </summary>
		/// <param name="objectType">The type of the object.</param>
		/// <param name="properties">Property names and values.</param>
		public static string ToString(Type objectType, params (string propName, object propValue)[] properties)
		{
			if(properties == null) {
				throw new ArgumentNullException(nameof(properties));
			}

			string stringRepresentation = objectType.Name;
			if(properties.Any()) {
				stringRepresentation += ": "
					+ string.Join(
						separator: "; ",
						values: properties
							.Select(((string Name, object Value) prop) =>
							{
								return $"{prop.Name}={prop.Value?.ToString()}";
							}));
			}
			return stringRepresentation;
		}
	}
}
