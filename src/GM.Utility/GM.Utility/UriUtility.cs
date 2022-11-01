/*
MIT License

Copyright (c) 2022 Gregor Mohorko

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
Created: 2022-03-09
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GM.Utility
{
    /// <summary>
    /// Utilities for <see cref="Uri"/>.
    /// </summary>
	public static class UriUtility
    {
        /// <summary>
        /// Combines the base uri and path segments into a combined uri.
        /// </summary>
        /// <param name="uri">The base uri.</param>
        /// <param name="segments">The path segments.</param>
        public static Uri Combine(string uri, params string[] segments)
        {
            return new Uri(uri).Append(segments);
        }

        /// <summary>
        /// Appends additional path segments to the this uri.
        /// </summary>
        /// <param name="uri">The base uri.</param>
        /// <param name="paths">Additional path segments.</param>
        public static Uri Append(this Uri uri, params string[] paths)
        {
            if(uri == null) {
                throw new ArgumentNullException(nameof(uri));
			}
            if(paths == null) {
                throw new ArgumentNullException(nameof(paths));
            }

            // taken from: https://stackoverflow.com/a/7993235/6277755

            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }
    }
}
