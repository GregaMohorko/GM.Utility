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
Created: 2018-3-29
Author: GregaMohorko
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GM.Utility.Test
{
	[TestClass]
	public class DefaultedObjectTest
	{
		private class MyClass:DefaultedObject<string>
		{
			internal const string DEFAULT = "DEFAULT TEXT";
			protected override string Default => DEFAULT;

#pragma warning disable 0649
			public int IntField;
			public int IntProperty { get; set; }

			public static string StaticPublicStringField;
			internal static string staticInternalStringField;
			protected static string StaticProtectedStringField;
			public static string GetStaticProtectedStringField => StaticProtectedStringField;
			private static string staticPrivateStringField;
			public static string GetStaticPrivateStringField => staticPrivateStringField;

			public string PublicStringField;
			internal string InternalStringField;
			protected string ProtectedStringField;
			public string GetProtectedStringField => ProtectedStringField;
			private string PrivateStringField;
			public string GetPrivateStringField => PrivateStringField;
#pragma warning restore 0649

			public string PublicStringProperty { get; set; }
			public string PublicStringPropertyWithInternalSetter { get; internal set; }
			public string PublicStringPropertyWithProtectedSetter { get; protected set; }
			public string PublicStringPropertyWithPrivateSetter { get; private set; }
			internal string InternalStringProperty { get; set; }
			protected string ProtectedStringProperty { get; set; }
			public string GetProtectedStringProperty => ProtectedStringProperty;
			private string PrivateStringProperty { get; set; }
			public string GetPrivateStringProperty => PrivateStringProperty;
		}

		[TestMethod]
		public void Test()
		{
			var @object = new MyClass();
			Assert.AreEqual(default(int), @object.IntField);
			Assert.AreEqual(default(int), @object.IntProperty);
			Assert.AreEqual(MyClass.DEFAULT, @object.PublicStringField);
			Assert.AreEqual(MyClass.DEFAULT, @object.InternalStringField);
			Assert.AreEqual(MyClass.DEFAULT, @object.GetProtectedStringField);
			Assert.AreEqual(MyClass.DEFAULT, @object.GetPrivateStringField);
			Assert.AreEqual(MyClass.DEFAULT, @object.PublicStringProperty);
			Assert.AreEqual(MyClass.DEFAULT, @object.PublicStringPropertyWithInternalSetter);
			Assert.AreEqual(MyClass.DEFAULT, @object.PublicStringPropertyWithProtectedSetter);
			Assert.AreEqual(MyClass.DEFAULT, @object.PublicStringPropertyWithPrivateSetter);
			Assert.AreEqual(MyClass.DEFAULT, @object.InternalStringProperty);
			Assert.AreEqual(MyClass.DEFAULT, @object.GetProtectedStringProperty);
			Assert.AreEqual(MyClass.DEFAULT, @object.GetPrivateStringProperty);
			Assert.AreEqual(null, MyClass.StaticPublicStringField);
			Assert.AreEqual(null, MyClass.staticInternalStringField);
			Assert.AreEqual(null, MyClass.GetStaticProtectedStringField);
			Assert.AreEqual(null, MyClass.GetStaticPrivateStringField);
		}
	}
}
