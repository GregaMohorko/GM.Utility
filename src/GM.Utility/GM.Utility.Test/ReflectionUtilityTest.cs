/*
MIT License

Copyright (c) 2018 Grega Mohorko

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

Project: GM.Utility.Test
Created: 2018-12-10
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class ReflectionUtilityTest
	{
		private class ReflectionExampleClass
		{
#pragma warning disable 0649
			public string PublicField;
			internal string InternalField;
			protected internal string ProtectedInternalField;
			protected string ProtectedField;
			public string GetProtectedField => ProtectedField;
			private string privateField;
			public string GetPrivateField => privateField;
#pragma warning restore 0649
		}

		[TestMethod]
		public void SetField()
		{
			var obj = new ReflectionExampleClass();

			ReflectionUtility.SetField(obj, nameof(ReflectionExampleClass.PublicField), "publicValue");
			Assert.AreEqual("publicValue", obj.PublicField);
			obj.SetField(nameof(ReflectionExampleClass.InternalField), "internalValue");
			Assert.AreEqual("internalValue", obj.InternalField);
			obj.SetField(nameof(ReflectionExampleClass.ProtectedInternalField), "protectedInternalValue");
			Assert.AreEqual("protectedInternalValue", obj.ProtectedInternalField);
			obj.SetField("ProtectedField", "protectedValue");
			Assert.AreEqual("protectedValue", obj.GetProtectedField);
			obj.SetField("privateField", "privateValue");
			Assert.AreEqual("privateValue", obj.GetPrivateField);
		}
	}
}
