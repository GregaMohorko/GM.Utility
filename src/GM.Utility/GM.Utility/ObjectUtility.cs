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
Created: 2017-10-27
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for object.
	/// </summary>
	public static class ObjectUtility
	{
		private static readonly MethodInfo MemberwiseCloneMethod;

		static ObjectUtility()
		{
			MemberwiseCloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		/// <summary>
		/// Creates a deep copy (new instance with new instances of properties).
		/// <para>
		/// References:
		/// https://raw.githubusercontent.com/Burtsev-Alexey/net-object-deep-copy/master/ObjectExtensions.cs
		/// </para>
		/// </summary>
		/// <param name="original">The original object to copy.</param>
		public static object DeepCopy(this object original)
		{
			return DeepCopyInternal(original, new Dictionary<object, object>(new ReferenceEqualityComparer()));
		}

		private static object DeepCopyInternal(object original, IDictionary<object, object> visited)
		{
			if(original == null) {
				return null;
			}

			Type typeToReflect = original.GetType();

			if(typeToReflect.IsPrimitive()) {
				return original;
			}

			if(visited.ContainsKey(original)) {
				return visited[original];
			}

			if(typeof(Delegate).IsAssignableFrom(typeToReflect)) {
				return null;
			}

			object cloneObject = MemberwiseCloneMethod.Invoke(original, null);

			if(typeToReflect.IsArray) {
				Type arrayType = typeToReflect.GetElementType();
				if(!arrayType.IsPrimitive()) {
					Array clonedArray = (Array)cloneObject;
					clonedArray.ForEach((array, indices) => array.SetValue(DeepCopyInternal(clonedArray.GetValue(indices), visited), indices));
				}
			}

			visited.Add(original, cloneObject);

			DeepCopyFields(original, visited, cloneObject, typeToReflect);

			RecursiveDeepCopyBaseTypePrivateFields(original, visited, cloneObject, typeToReflect);

			return cloneObject;
		}

		private static void DeepCopyFields(object original, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			foreach(FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags)) {
				if(filter != null && !filter(fieldInfo)) {
					continue;
				}

				if(fieldInfo.FieldType.IsPrimitive()) {
					continue;
				}

				object originalFieldValue = fieldInfo.GetValue(original);
				object clonedFieldValue = DeepCopyInternal(originalFieldValue, visited);

				fieldInfo.SetValue(cloneObject, clonedFieldValue);
			}
		}

		private static void RecursiveDeepCopyBaseTypePrivateFields(object original, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if(typeToReflect.BaseType != null) {
				RecursiveDeepCopyBaseTypePrivateFields(original, visited, cloneObject, typeToReflect.BaseType);
				DeepCopyFields(original, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, fieldInfo => fieldInfo.IsPrivate);
			}
		}

		private class ReferenceEqualityComparer : EqualityComparer<object>
		{
			public override bool Equals(object x, object y)
			{
				return ReferenceEquals(x, y);
			}

			public override int GetHashCode(object obj)
			{
				if(obj == null) {
					return 0;
				}

				return obj.GetHashCode();
			}
		}
	}
}
