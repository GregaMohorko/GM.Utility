/*
MIT License

Copyright (c) 2024 Gregor Mohorko

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

Project: GM.Utility.Testing.Unit
Created: 2024-7-31
Author: grega
*/

namespace GM.Utility.Testing.Unit;

public class ReflectionUtilityTests
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

	[Fact]
	public void AreAllPropertiesEqual()
	{
		_ = Assert.Throws<ArgumentNullException>(() => ReflectionUtility.AreAllPropertiesEqual(null));
		// at least two objects must be provided
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[0]));
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[1]));
		// null objects are not allowed
		_ = Assert.Throws<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[2]));
		_ = Assert.Throws<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new ReflectionExampleClass[1024]));
		// all objects must be of the same type
		_ = Assert.Throws<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[2] { "string", 42 }));
		// primitive types are not allowed
		_ = Assert.Throws<ArgumentException>(() => ReflectionUtility.AreAllPropertiesEqual(new object[2] { "string", "string" }));

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
		Assert.True(ReflectionUtility.AreAllPropertiesEqual(objects));

		objects.ForEach(obj => obj.PublicProperty = "some value");
		Assert.True(ReflectionUtility.AreAllPropertiesEqual(objects));

		// set different public readonly properties
		for(int i = 0; i < objects.Length; ++i) {
			objects[i].SetField("privateField", $"Public readonly property {i}");
		}
		Assert.False(ReflectionUtility.AreAllPropertiesEqual(objects));

		// reset public readonly properties
		objects.ForEach(obj => obj.SetField("privateField", "same readonly value"));
		Assert.True(ReflectionUtility.AreAllPropertiesEqual(objects));

		objects[0].PublicProperty = "different value";
		Assert.False(ReflectionUtility.AreAllPropertiesEqual(objects));
	}

	[Fact]
	public void SetField()
	{
		var obj = new ReflectionExampleClass();

		ReflectionUtility.SetField(obj, nameof(ReflectionExampleClass.PublicField), "publicValue");
		Assert.Equal("publicValue", obj.PublicField);
		obj.SetField(nameof(ReflectionExampleClass.InternalField), "internalValue");
		Assert.Equal("internalValue", obj.InternalField);
		obj.SetField(nameof(ReflectionExampleClass.ProtectedInternalField), "protectedInternalValue");
		Assert.Equal("protectedInternalValue", obj.ProtectedInternalField);
		obj.SetField("ProtectedField", "protectedValue");
		Assert.Equal("protectedValue", obj.GetProtectedField);
		obj.SetField("privateField", "privateValue");
		Assert.Equal("privateValue", obj.GetPrivateField);
		obj.SetField("privateReadonlyField", "privateReadonlyValue");
		Assert.Equal("privateReadonlyValue", obj.GetPrivateReadonlyField);
	}
}
