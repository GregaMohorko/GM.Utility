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

Project: GM.Utility.Test
Created: 2018-12-10
Author: Gregor Mohorko
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
#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable 0649 // never assigned to
#pragma warning disable IDE0051 // Remove unused private members
			public string PublicField;
			internal string InternalField;
			protected internal string ProtectedInternalField;
			protected string ProtectedField;
			public string GetProtectedField => ProtectedField;
			private string privateField;
			public string GetPrivateField => privateField;
			private readonly string privateReadonlyField;
			public string GetPrivateReadonlyField => privateReadonlyField;

			public string PublicProperty { get; set; }
			public string PublicReadonlyProperty { get; } = "Public Readonly Property Value";
			internal string InternalProperty { get; set; }
			protected string ProtectedProperty { get; set; }
			private string PrivateProperty { get; set; }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable 0649 // never assigned to
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore IDE0032 // Use auto property
		}

		[TestMethod]
		public void AreAllPropertiesEqual()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => ReflectionUtility.AreAllPropertiesEqual(null));
			// at least two objects must be provided
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[0]));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[1]));
			// null objects are not allowed
			_ = Assert.ThrowsException<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[2]));
			_ = Assert.ThrowsException<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new ReflectionExampleClass[1024]));
			// all objects must be of the same type
			_ = Assert.ThrowsException<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[2] { "string", 42 }));
			// primitive types are not allowed
			_ = Assert.ThrowsException<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[2] { "string", "string" }));

			var objects = new ReflectionExampleClass[2]
			{
				new ReflectionExampleClass
				{
					// different field values and internal properties
					PublicField = "Public field 0",
					InternalField = "Internal field 0",
					InternalProperty = "Internal property 0"
				},
				new ReflectionExampleClass
				{
					// different field values and internal properties
					PublicField = "Public field 1",
					InternalField = "Internal field 1",
					InternalProperty = "Internal property 1"
				}
			};

			// set different values to private and protected properties
			for(int i = 0; i < objects.Length; ++i) {
				objects[i].SetProperty("PrivateProperty", $"Private property {i}");
				objects[i].SetProperty("ProtectedProperty", $"Protected property {i}");
			}

			// all public properties are null (have not been set)
			Assert.IsTrue(ReflectionUtility.AreAllPropertiesEqual(objects));

			objects.ForEach(obj => obj.PublicProperty = "some value");
			Assert.IsTrue(ReflectionUtility.AreAllPropertiesEqual(objects));

			// set different public readonly properties
			for(int i = 0; i < objects.Length; ++i) {
				objects[i].SetField("privateField", $"Public readonly property {i}");
			}
			Assert.IsFalse(ReflectionUtility.AreAllPropertiesEqual(objects));

			// reset public readonly properties
			objects.ForEach(obj => obj.SetField("privateField", "same readonly value"));
			Assert.IsTrue(ReflectionUtility.AreAllPropertiesEqual(objects));

			objects[0].PublicProperty = "different value";
			Assert.IsFalse(ReflectionUtility.AreAllPropertiesEqual(objects));
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
			obj.SetField("privateReadonlyField", "privateReadonlyValue");
			Assert.AreEqual("privateReadonlyValue", obj.GetPrivateReadonlyField);
		}
	}
}
