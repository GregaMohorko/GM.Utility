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
Created: 2017-12-19
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility.Patterns
{
	/// <summary>
	/// Base class for a thread-safe (creation wise) singleton class.
	/// <para>It will use the parameterless constructor to create the instance.</para>
	/// </summary>
	/// <typeparam name="T">The type of the actual singleton class.</typeparam>
	public abstract class Singleton<T> where T : Singleton<T>
	{
		private static readonly object _lock_instance = new object();
		private static T _instance;
		/// <summary>
		/// Gets the instance of this singleton.
		/// </summary>
		public static T Instance
		{
			get
			{
				if(_instance == null) {
					lock(_lock_instance) {
						if(_instance == null) {
							// create an instance of T using the public, protected or private parameterless constructor
							_instance = (T)Activator.CreateInstance(typeof(T), true);
						}
					}
				}
				return _instance;
			}
		}
	}
}
