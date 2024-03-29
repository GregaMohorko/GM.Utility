﻿/*
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
Created: 2017-10-27
Author: Gregor Mohorko
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Reflection utilities.
	/// </summary>
	public static class ReflectionUtility
	{
		#region ASSEMBLY
		/// <summary>
		/// Specifies the type of the assembly.
		/// </summary>
		public enum AssemblyType
		{
			/// <summary>
			/// Unknown.
			/// </summary>
			UNKNOWN = 0,
			/// <summary>
			/// Represents the entry assembly, which is usually the assembly of the application.
			/// </summary>
			APPLICATION = 1,
			/// <summary>
			/// Represents the assembly of the library where this enum is defined.
			/// </summary>
			LIBRARY = 2,
			/// <summary>
			/// Represents the currently running assembly.
			/// </summary>
			CURRENT = 3
		}

		/// <summary>
		/// Structure with assembly information.
		/// </summary>
		public struct AssemblyInformation
		{
			/// <summary>
			/// Title.
			/// </summary>
			public readonly string Title;
			/// <summary>
			/// Description.
			/// </summary>
			public readonly string Description;
			/// <summary>
			/// Company.
			/// </summary>
			public readonly string Company;
			/// <summary>
			/// Product.
			/// </summary>
			public readonly string Product;
			/// <summary>
			/// Copyright.
			/// </summary>
			public readonly string Copyright;
			/// <summary>
			/// Trademark.
			/// </summary>
			public readonly string Trademark;
			/// <summary>
			/// Version.
			/// </summary>
			public readonly Version Version;

			/// <summary>
			/// Creates a new instance of AssemblyInformation.
			/// </summary>
			/// <param name="title">Title.</param>
			/// <param name="description">Description.</param>
			/// <param name="company">Company.</param>
			/// <param name="product">Product.</param>
			/// <param name="copyright">Copyright.</param>
			/// <param name="trademark">Trademark.</param>
			/// <param name="version">Version.</param>
			public AssemblyInformation(string title, string description, string company, string product, string copyright, string trademark, Version version)
			{
				Title = title;
				Description = description;
				Company = company;
				Product = product;
				Copyright = copyright;
				Trademark = trademark;
				Version = version;
			}
		}

		/// <summary>
		/// Gets the assembly of the specified type.
		/// </summary>
		/// <param name="assemblyType">The type of the assembly to look for.</param>
		public static Assembly GetAssembly(AssemblyType assemblyType)
		{
			Assembly assembly;

			switch(assemblyType) {
				case AssemblyType.APPLICATION:
					assembly = Assembly.GetEntryAssembly();
					break;
				case AssemblyType.LIBRARY:
					assembly = Assembly.GetExecutingAssembly();
					break;
				case AssemblyType.CURRENT:
					assembly = null;
					break;
				default:
					throw new ArgumentException($"Unsupported assembly type '{assemblyType}'.");
			}

			if(assembly == null) {
				assembly = Assembly.GetCallingAssembly();
			}
			if(assembly == null) {
				throw new Exception("Assembly information could not be found.");
			}

			return assembly;
		}

		/// <summary>
		/// Gets the assembly information of the specified type.
		/// <para>
		/// AssemblyType.CURRENT and AssemblyType.APPLICATION are not allowed.
		/// </para>
		/// </summary>
		/// <param name="assemblyType">The type of the assembly to look for.</param>
		public static AssemblyInformation GetAssemblyInformation(AssemblyType assemblyType)
		{
			if(assemblyType == AssemblyType.CURRENT || assemblyType == AssemblyType.APPLICATION) {
				throw new InvalidEnumArgumentException("AssemblyType.Current and AssemblyType.Application are not allowed when getting the assembly information. Use GetAssembly() to get the assembly first, then call GetAssemblyInformation(Assembly) with it.");
			}
			var assembly = GetAssembly(assemblyType);
			return GetAssemblyInformation(assembly);
		}

		/// <summary>
		/// Gets the assembly information of the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly from which to extract the information from.</param>
		public static AssemblyInformation GetAssemblyInformation(this Assembly assembly)
		{
			string title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
			string description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
			string company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
			string product = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
			string copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
			string trademark = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark;

			string versionS;
			AssemblyVersionAttribute assemblyVersion = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
			if(assemblyVersion != null) {
				versionS = assemblyVersion.Version;
			} else {
				AssemblyFileVersionAttribute fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
				versionS = fileVersion?.Version;
			}
			Version version = null;
			if(versionS != null) {
				version = Version.Parse(versionS);
			}

			return new AssemblyInformation(title, description, company, product, copyright, trademark, version);
		}

		/// <summary>
		/// Gets the local (operating-system wise) directory path of the assembly that defines the specified type.
		/// </summary>
		/// <param name="type">The type that is defined in the assembly of which to get the local directory path.</param>
		public static string GetAssemblyDirectoryLocalPath(this Type type)
		{
			return GetAssemblyDirectoryLocalPath(type.Assembly);
		}

		/// <summary>
		/// Gets the local (operating-system wise) directory path of the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static string GetAssemblyDirectoryLocalPath(this Assembly assembly)
		{
			string filePath = GetAssemblyFileLocalPath(assembly);
			return Path.GetDirectoryName(filePath);
		}

		/// <summary>
		/// Gets the local (operating-system wise) file (.dll) path of the assembly that defines the specified type.
		/// </summary>
		/// <param name="type">The type that is defined in the assembly of which to get the local file path.</param>
		public static string GetAssemblyFileLocalPath(this Type type)
		{
			return GetAssemblyFileLocalPath(type.Assembly);
		}

		/// <summary>
		/// Gets the local (operating-system wise) file (.dll) path of the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static string GetAssemblyFileLocalPath(this Assembly assembly)
		{
			string codebase = assembly.CodeBase;
			var uri = new Uri(codebase, UriKind.Absolute);
			return uri.LocalPath;
		}
		#endregion // Assembly

		/// <summary>
		/// Calls the static constructor of this type.
		/// </summary>
		/// <param name="type">The type of which to call the static constructor.</param>
		public static void CallStaticConstructor(this Type type)
		{
			// https://stackoverflow.com/questions/11520829/explicitly-call-static-constructor/29511342#29511342
			// https://stackoverflow.com/questions/2654010/how-can-i-run-a-static-constructor/2654684#2654684
			RuntimeHelpers.RunClassConstructor(type.TypeHandle);
		}

		/// <summary>
		/// Gets the method that called the current method where this method is used. This does not work when used in async methods.
		/// <para>
		/// Note that because of compiler optimization, you should add <see cref="MethodImplAttribute"/> to the method where this method is used and use the <see cref="MethodImplOptions.NoInlining"/> value.
		/// </para>
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static MethodBase GetCallingMethod()
		{
			return new StackFrame(2, false)?.GetMethod();
		}

		/// <summary>
		/// Gets the type that called the current method where this method is used.
		/// <para>
		/// Note that because of compiler optimization, you should add <see cref="MethodImplAttribute"/> to the method where this method is used and use the <see cref="MethodImplOptions.NoInlining"/> value.
		/// </para>
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetCallingType()
		{
			return new StackFrame(2, false)?.GetMethod()?.DeclaringType;
		}

		/// <summary>
		/// Gets all the namespaces in this assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static IEnumerable<string> GetNamespaces(this Assembly assembly)
		{
			if(assembly == null) {
				throw new ArgumentNullException(nameof(assembly));
			}
			return assembly.GetTypes().Select(t => t.Namespace);
		}

		/// <summary>
		/// Gets the types defined in this assembly and in the specified namespace.
		/// </summary>
		/// <param name="assembly">The assembly from which to get the types.</param>
		/// <param name="namespace">The namespace in which the types are defined.</param>
		public static IEnumerable<Type> GetTypesInNamespace(this Assembly assembly, string @namespace)
		{
			return assembly.GetTypes().Where(t => t.Namespace == @namespace);
		}

		/// <summary>
		/// Gets the <see cref="Type"/> with the specified name, performing a case-sensitive search. If the type is not in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll, this method will look in the assemblies that <seealso cref="AppDomain.GetAssemblies()"/> returns for the current domain.
		/// </summary>
		/// <param name="typeName">The assembly-qualified name of the type to get. If the type is in the currently executing assembly or in mscorlib.dll/System.Private.CoreLib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
		public static Type GetType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if(type == null) {
				foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
					type = assembly.GetType(typeName);
					if(type != null) {
						break;
					}
				}
			}
			return type ?? throw new ArgumentException($"Could not find a type with name '{typeName}'.", nameof(typeName));
		}

		/// <summary>
		/// Returns all the public properties of this object whose property type is <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the properties.</typeparam>
		/// <param name="obj">The object.</param>
		public static IEnumerable<PropertyInfo> GetAllPropertiesOfType<T>(this object obj)
		{
			return GetAllPropertiesOfType<T>(obj.GetType());
		}

		/// <summary>
		/// Returns all the public properties of this type whose property type is <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the properties.</typeparam>
		/// <param name="type">The type of which to get the properties.</param>
		public static IEnumerable<PropertyInfo> GetAllPropertiesOfType<T>(this Type type)
		{
			return type.GetProperties().Where(pi => pi.PropertyType == typeof(T));
		}

		/// <summary>
		/// Returns all the values of public properties of this object whose property type is <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of the properties.</typeparam>
		/// <param name="obj">The object.</param>
		public static IEnumerable<T> GetAllPropertiesValuesOfType<T>(this object obj)
		{
			return GetAllPropertiesOfType<T>(obj).Select(pi => (T)pi.GetValue(obj));
		}

		/// <summary>
		/// Gets the value of the property or field with the specified name in this object or type.
		/// </summary>
		/// <param name="obj">The object or type that has the property or field.</param>
		/// <param name="propertyOrFieldName">The name of the property or field.</param>
		public static object GetValue(this object obj, string propertyOrFieldName)
		{
			if(obj == null) {
				throw new ArgumentNullException(nameof(obj));
			}
			Type type;
			if(obj is Type typeObj) {
				// getting static properties/fields
				type = typeObj;
			} else {
				type = obj.GetType();
			}
			PropertyInfo property = type.GetProperty(propertyOrFieldName);
			if(property != null) {
				return property.GetValue(obj);
			}
			FieldInfo field = type.GetField(propertyOrFieldName);
			if(field != null) {
				return field.GetValue(obj);
			}
			throw new Exception($"'{propertyOrFieldName}' is neither a property or a field of type '{type}'.");
		}

		/// <summary>
		/// Sets the value of the property or field with the specified name in this object or type.
		/// </summary>
		/// <param name="obj">The objector type that has the property or field.</param>
		/// <param name="propertyOrFieldName">The name of the property or field.</param>
		/// <param name="value">The value to set.</param>
		public static void SetValue(this object obj, string propertyOrFieldName, object value)
		{
			if(obj == null) {
				throw new ArgumentNullException(nameof(obj));
			}
			Type type;
			if(obj is Type typeObj) {
				// setting static properties/fields
				type = typeObj;
			} else {
				type = obj.GetType();
			}
			PropertyInfo property = type.GetProperty(propertyOrFieldName);
			if(property != null) {
				property.SetValue(obj, value);
				return;
			}
			FieldInfo field = type.GetField(propertyOrFieldName);
			if(field != null) {
				field.SetValue(obj, value);
				return;
			}
			throw new Exception($"'{propertyOrFieldName}' is neither a property or a field of type '{type}'.");
		}

		/// <summary>
		/// Gets the value of the specified property in the object.
		/// </summary>
		/// <param name="obj">The object that has the property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static object GetPropertyValue(this object obj, string propertyName, BindingFlags? bindingAttr = null)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName, bindingAttr);
			return property.GetValue(obj);
		}

		/// <summary>
		/// Gets the value of the specified field in the object.
		/// </summary>
		/// <param name="obj">The object that has the field.</param>
		/// <param name="fieldName">The name of the field.</param>
		public static object GetFieldValue(this object obj, string fieldName)
		{
			FieldInfo field = GetFieldInfo(obj, fieldName);
			return field.GetValue(obj);
		}

		/// <summary>
		/// Returns the value of the property/field of the provided object with the specified path.
		/// <para>The path can contain the dot (.) character to search deeper into sub-properties/fields.</para>
		/// <para>The path can also contain the [] brackets with an index for getting an element at the specified position inside the brackets.</para>
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="path">The path of the property/field. Can contain dot '.' character and '[]' brackets with an index for lists.</param>
		public static object GetValueFromPath(object obj, string path)
		{
			if(obj == null) {
				throw new ArgumentNullException(nameof(obj));
			}
			if(path == null) {
				throw new ArgumentNullException(nameof(path));
			}
			if(string.IsNullOrWhiteSpace(path)) {
				throw new ArgumentException("The path must not be empty.", nameof(path));
			}

			object current = obj;

			string[] subPaths = path.Split('.');
			foreach(string subPath in subPaths) {
				if(subPath.EndsWith("]")) {
					int start = subPath.LastIndexOf('[');
					if(start >= 0) {
						string subPathWithoutIndex = subPath.Substring(0, start);
						string indexS = subPath.Substring(start + 1, subPath.Length - start - 2);
						if(int.TryParse(indexS, out int index)) {
							var list = (IList)GetValue(current, subPathWithoutIndex);
							current = list[index];
							continue;
						}
					}
				}
				current = GetValue(current, subPath);
			}

			return current;
		}

		/// <summary>
		/// Sets the value to the property/field of the provided object with the specified path.
		/// <para>The path can contain the dot (.) character to search deeper into sub-properties/fields.</para>
		/// <para>The path can also contain the [] brackets with an index for getting an element at the specified position inside the brackets.</para>
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="path">The path of the property/field. Can contain dot '.' character and '[]' brackets with an index for lists.</param>
		/// <param name="value">The value to set.</param>
		public static void SetValueFromPath(object obj, string path, object value)
		{
			if(obj == null) {
				throw new ArgumentNullException(nameof(obj));
			}
			if(path == null) {
				throw new ArgumentNullException(nameof(path));
			}
			if(string.IsNullOrWhiteSpace(path)) {
				throw new ArgumentException("The path must not be empty.", nameof(path));
			}

			object current = obj;

			string[] subPaths = path.Split('.');
			foreach(string subPath in subPaths.Take(subPaths.Length - 1)) {
				if(subPath.EndsWith("]")) {
					int start = subPath.LastIndexOf('[');
					if(start >= 0) {
						string subPathWithoutIndex = subPath.Substring(0, start);
						string indexS = subPath.Substring(start + 1, subPath.Length - start - 2);
						if(int.TryParse(indexS, out int index)) {
							var list = (IList)GetValue(current, subPathWithoutIndex);
							current = list[index];
							continue;
						}
					}
				}
				current = GetValue(current, subPath);
			}

			SetValue(current, subPaths.Last(), value);
		}

		/// <summary>
		/// Gets the type of the specified property in the type.
		/// <para>
		/// If the type is nullable, this function gets its generic definition. To get the real type, use <see cref="GetPropertyTypeReal(Type, string, BindingFlags?)"/>.
		/// </para>
		/// </summary>
		/// <param name="type">The type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static Type GetPropertyType(this Type type, string propertyName, BindingFlags? bindingAttr = null)
		{
			Type propertyType = GetPropertyTypeReal(type, propertyName, bindingAttr);

			// get the generic type of nullable, not THE nullable
			if(propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
				propertyType = GetGenericFirst(propertyType);
			}

			return propertyType;
		}

		/// <summary>
		/// Gets the type of the specified property in the type.
		/// </summary>
		/// <param name="type">The type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static Type GetPropertyTypeReal(this Type type, string propertyName, BindingFlags? bindingAttr = null)
		{
			PropertyInfo property = GetPropertyInfo(type, propertyName, bindingAttr);
			return property.PropertyType;
		}

		/// <summary>
		/// Gets the property information by name for the type of the object.
		/// </summary>
		/// <param name="obj">Object with a type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static PropertyInfo GetPropertyInfo(this object obj, string propertyName, BindingFlags? bindingAttr = null)
		{
			return GetPropertyInfo(obj.GetType(), propertyName, bindingAttr);
		}

		/// <summary>
		/// Gets the property information by name for the type.
		/// </summary>
		/// <param name="type">Type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static PropertyInfo GetPropertyInfo(this Type type, string propertyName, BindingFlags? bindingAttr = null)
		{
			PropertyInfo property;
			if(bindingAttr == null) {
				property = type.GetProperty(propertyName);
			} else {
				property = type.GetProperty(propertyName, bindingAttr.Value);
			}
			if(property == null) {
				throw new Exception(string.Format("The provided property name ({0}) does not exist in type '{1}'.", propertyName, type.ToString()));
			}

			return property;
		}

		/// <summary>
		/// Gets the field information by name for the type of the object.
		/// </summary>
		/// <param name="obj">Object with a type that has the specified field.</param>
		/// <param name="fieldName">The name of the field.</param>
		public static FieldInfo GetFieldInfo(this object obj, string fieldName)
		{
			return GetFieldInfo(obj.GetType(), fieldName);
		}

		/// <summary>
		/// Gets the field information by name for the type.
		/// </summary>
		/// <param name="type">Type that has the specified field.</param>
		/// <param name="fieldName">The name of the field.</param>
		public static FieldInfo GetFieldInfo(this Type type, string fieldName)
		{
			FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if(field == null) {
				throw new Exception($"The provided property name ({fieldName}) does not exist in type '{type}'.");
			}
			return field;
		}

		/// <summary>
		/// Returns the first definition of generic type of this generic type.
		/// </summary>
		/// <param name="type">The type from which to get the generic type.</param>
		public static Type GetGenericFirst(this Type type)
		{
			return type.GetGenericArguments()[0];
		}

		/// <summary>
		/// Gets the constants defined in this type.
		/// </summary>
		/// <param name="type">The type from which to get the constants.</param>
		/// <param name="includeInherited">Determines whether or not to include inherited constants.</param>
		public static IEnumerable<FieldInfo> GetConstants(this Type type, bool includeInherited)
		{
			BindingFlags bindingFlags;
			if(includeInherited) {
				bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
			} else {
				bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
			}

			FieldInfo[] fields = type.GetFields(bindingFlags);

			return fields.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
		}

		/// <summary>
		/// Sets the specified property to the provided value in the object.
		/// <para>By default, uses <see cref="BindingFlags.Instance"/> | <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.NonPublic"/>.</para>
		/// </summary>
		/// <param name="obj">The object with the property.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="value">The value to set the property to.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null. Default value is <see cref="BindingFlags.Instance"/> | <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.NonPublic"/>.</param>
		public static void SetProperty(this object obj, string propertyName, object value, BindingFlags? bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName, bindingAttr);
			property.SetValue(obj, value);
		}

		/// <summary>
		/// Sets the specified static property of the specified type to the provided value.
		/// <para><see cref="BindingFlags.Static"/> is always included.</para>
		/// <para>By default, uses <see cref="BindingFlags.Public"/> | <see cref="BindingFlags.NonPublic"/>.</para>
		/// </summary>
		/// <param name="type">The type with the static property.</param>
		/// <param name="propertyName">The name of the static property to set.</param>
		/// <param name="value">The value to set the static property to.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static void SetProperty(Type type, string propertyName, object value, BindingFlags? bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		{
			bindingAttr |= BindingFlags.Static;
			PropertyInfo property = GetPropertyInfo(type, propertyName, bindingAttr);
			property.SetValue(null, value);
		}

		/// <summary>
		/// Sets the specified field to the provided value in the object.
		/// <para>Works for public, protected, private and readonly fields.</para>
		/// </summary>
		/// <param name="obj">The object with the field.</param>
		/// <param name="fieldName">The name of the field to set.</param>
		/// <param name="value">The value to set the field to.</param>
		public static void SetField(this object obj, string fieldName, object value)
		{
			FieldInfo field = GetFieldInfo(obj, fieldName);
			field.SetValue(obj, value);
		}

		/// <summary>
		/// Sets the specified property to a value that will be extracted from the provided string value using the <see cref="TypeDescriptor.GetConverter(Type)"/> and <see cref="TypeConverter.ConvertFromString(string)"/>.
		/// </summary>
		/// <param name="obj">The object with the property.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="valueAsString">The string representation of the value to set to the property.</param>
		/// <param name="bindingAttr">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted. -or- Zero, to return null.</param>
		public static void SetPropertyFromString(this object obj, string propertyName, string valueAsString, BindingFlags? bindingAttr = null)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName, bindingAttr);
			TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
			object value = converter.ConvertFromString(valueAsString);
			property.SetValue(obj, value);
		}

		/// <summary>
		/// Sets all fields and properties of the specified type in the provided object to the specified value.
		/// <para>Internal, protected and private fields are included, static are not.</para>
		/// </summary>
		/// <typeparam name="T">The type of the properties.</typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="value">The value to set the properties to.</param>
		public static void SetAllPropertiesOfType<T>(this object obj, T value)
		{
			Type type = obj.GetType();

			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach(FieldInfo field in fields) {
				if(field.FieldType == typeof(T)) {
					field.SetValue(obj, value);
				}
			}
		}

		/// <summary>
		/// Determines whether or not this object has a property with the specified name.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="includeInherited">Determines whether of not to include inherited properties.</param>
		public static bool HasProperty(this object obj, string propertyName, bool includeInherited)
		{
			Type type = obj.GetType();
			return HasProperty(type, propertyName, includeInherited);
		}

		/// <summary>
		/// Determines whether or not this type has a property with the specified name.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="includeInherited">Determines whether of not to include inherited properties.</param>
		public static bool HasProperty(this Type type, string propertyName, bool includeInherited)
		{
			BindingFlags bindingAttr;
			if(includeInherited) {
				bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
			} else {
				bindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
			}

			PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingAttr);
			return propertyInfo != null;
		}

		/// <summary>
		/// Determines whether all the public properties of the provided objects have the same value. Compares using the <see cref="object.Equals(object)"/> method.
		/// <para>All objects must be of the same type.</para>
		/// </summary>
		/// <param name="obj1">The first object whose properties are checked to be equal to others.</param>
		/// <param name="obj2">The second object whose properties are checked to be equal to thers.</param>
		/// <param name="objects">Any additional objects whose properties are checked to be equal.</param>
		/// <returns>True if all the public properties in the provided objects are equal.</returns>
		/// <exception cref="ArgumentException" />
		public static bool AreAllPropertiesEqual(object obj1, object obj2, params object[] objects)
		{
			object[] objectsToPassOn = new object[] { obj1, obj2 };
			if(!objects.IsNullOrEmpty()) {
				objectsToPassOn = objectsToPassOn.Concat(objects).ToArray();
			}
			return AreAllPropertiesEqual(objectsToPassOn);
		}

		/// <summary>
		/// Determines whether all the public properties of the provided objects have the same value. Compares using the <see cref="object.Equals(object)"/> method.
		/// <para>All objects must be of the same type.</para>
		/// </summary>
		/// <param name="objects">The objects whose properties are checked to be equal.</param>
		/// <returns>True if all the public properties in the provided objects are equal.</returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public static bool AreAllPropertiesEqual(object[] objects)
		{
			if(objects == null) {
				throw new ArgumentNullException(nameof(objects));
			}
			if(objects.Length < 2) {
				throw new ArgumentOutOfRangeException(nameof(objects), "At least two objects must be provided.");
			}
			if(objects.Any(obj => obj == null)) {
				throw new ArgumentException("Null objects are not allowed.");
			}
			if(!objects.AllSame(obj => obj.GetType())) {
				throw new ArgumentException("All objects must be of the same type.");
			}
			
			Type type = objects[0].GetType();
			if(IsPrimitive(type)) {
				throw new ArgumentException("Primitive types are not allowed.");
			}

			// go through all the public properties
			PropertyInfo[] properties = type.GetProperties();
			foreach(var property in properties) {
				// check that all objects have the same value of this property
				if(!objects.AllSame(obj => property.GetValue(obj))) {
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether this type implements the specified type.
		/// <para>Returns false if both types are the same.</para>
		/// <para>If the <paramref name="type"/> is generic (without specified generic type arguments), this method will return true if this type actually implements it (even though the <paramref name="type"/> is not directly assignable from this type).</para>
		/// </summary>
		/// <param name="source">The parent type.</param>
		/// <param name="type">The child type to check if the parent type implements it.</param>
		public static bool Implements(this Type source, Type type)
		{
			return Implements(source, type, out _);
		}

		/// <summary>
		/// Determines whether this type implements the specified type.
		/// <para>Returns false if both types are the same.</para>
		/// <para>If the <paramref name="type"/> is generic (without specified generic type arguments), this method will return true if this type actually implements it (even though the <paramref name="type"/> is not directly assignable from this type).</para>
		/// </summary>
		/// <param name="source">The parent type.</param>
		/// <param name="type">The child type to check if the parent type implements it.</param>
		/// <param name="genericType">If the <paramref name="type"/> is generic (without specified generic type arguments) and the result of this method is true, then this will represent the actual generic type that this type implements.</param>
		public static bool Implements(this Type source, Type type, out Type genericType)
		{
			if(source == null) {
				throw new ArgumentNullException(nameof(source));
			}
			if(type == null) {
				throw new ArgumentNullException(nameof(type));
			}

			genericType = null;

			if(type.Equals(source)) {
				return false;
			}

			if(type.IsAssignableFrom(source)) {
				// is directly assignable
				return true;
			}

			// check if type is generic and generic type arguments are not provided
			// in that case, it is still possible that the source type implements the type, but we need to check without generic type arguments

			if(!type.IsGenericType) {
				// type is not generic, nothing more to do
				return false;
			}
			if(type.GenericTypeArguments.Length > 0) {
				// type arguments are provided, nothing more to dos
				return false;
			}

			bool IsAssignableFromGeneric(Type parent, Type child)
			{
				if(child.IsGenericType) {
					Type genericChild = child.GetGenericTypeDefinition();
					if(parent.IsAssignableFrom(genericChild)) {
						return true;
					}
				}
				return false;
			}

			if(IsAssignableFromGeneric(type, source)) {
				return true;
			}

			if(!type.IsInterface) {
				// go through all the parent types
				Type currentType = source.BaseType;
				while(currentType != null) {
					if(IsAssignableFromGeneric(type, currentType)) {
						return true;
					}
					currentType = currentType.BaseType;
				}
			} else {
				// go through all the implemented interfaces
				foreach(Type implementedInterface in source.GetInterfaces()) {
					if(IsAssignableFromGeneric(type, implementedInterface)) {
						genericType = implementedInterface;
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Returns the generic type arguments that this type has specified when it implemented the specified generic parent type.
		/// </summary>
		/// <param name="source">The type that implemented the <paramref name="genericParentType"/>.</param>
		/// <param name="genericParentType">The generic type that this type implemented. Must be without generic type arguments.</param>
		public static Type[] GetGenericTypeArgumentsFor(this Type source, Type genericParentType)
		{
			if(genericParentType == null) {
				throw new ArgumentNullException(nameof(genericParentType));
			}
			if(!genericParentType.IsGenericType) {
				throw new ArgumentException($"'{nameof(genericParentType)}' must be generic.", nameof(genericParentType));
			}
			if(!genericParentType.GenericTypeArguments.IsNullOrEmpty()) {
				throw new ArgumentException($"'{nameof(genericParentType)}' must not have generic type arguments.", nameof(genericParentType));
			}
			if(!Implements(source, genericParentType, out Type implementedGenericParentType)) {
				throw new ArgumentException($"'{nameof(source)}' must implement '{nameof(genericParentType)}'. '{nameof(source)}'='{source.FullName}'. '{nameof(genericParentType)}'='{genericParentType.FullName}'");
			}
			return implementedGenericParentType.GenericTypeArguments;
		}

		/// <summary>
		/// Determines whether the specified type to check derives from this generic type.
		/// </summary>
		/// <param name="generic">The parent generic type.</param>
		/// <param name="toCheck">The type to check if it derives from the specified generic type.</param>
		public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while(toCheck != null && toCheck != typeof(object)) {
				Type current = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if(generic == current) {
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified object is of generic list type.
		/// </summary>
		/// <param name="obj">The object of which the type to determine.</param>
		public static bool IsGenericList(object obj)
		{
			if(obj == null) {
				return false;
			}
			return IsGenericList(obj.GetType());
		}

		/// <summary>
		/// Determines whether this type is a generic list.
		/// </summary>
		/// <param name="type">The type to determine.</param>
		public static bool IsGenericList(this Type type)
		{
			if(!typeof(IList).IsAssignableFrom(type)) {
				return false;
			}
			if(!type.IsGenericType) {
				return false;
			}
			if(!type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) {
				return false;
			}
			return true;
		}

		/// <summary>
		/// Determines whether this type is a primitive.
		/// </summary>
		/// <param name="type">The type.</param>
		public static bool IsPrimitive(this Type type)
		{
			return type.IsValueType
				|| type.IsPrimitive
				// below types are considered primitive
				|| type == typeof(string)
				|| type == typeof(decimal)
				|| type == typeof(DateTime)
				|| type == typeof(DateTimeOffset)
				|| type == typeof(TimeSpan)
				|| type == typeof(Guid)
				|| Convert.GetTypeCode(type) != TypeCode.Object;
		}

		/// <summary>
		/// Gets the default value of this type.
		/// </summary>
		/// <param name="type">The type for which to get the default value.</param>
		public static object GetDefault(this Type type)
		{
			// https://stackoverflow.com/questions/325426/programmatic-equivalent-of-defaulttype

			if(!type.IsValueType) {
				// is a reference type
				return null;
			}

			Func<object> f = GetDefaultGeneric<object>;
			return f.Method.GetGenericMethodDefinition().MakeGenericMethod(type).Invoke(null, null);
		}

		/// <summary>
		/// Gets the default value of the specified type.
		/// </summary>
		/// <typeparam name="T">The type for which to get the default value.</typeparam>
		public static T GetDefaultGeneric<T>()
		{
			return default;
		}
	}
}
