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
			if(assemblyType == AssemblyType.CURRENT || assemblyType==AssemblyType.APPLICATION) {
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
			string title = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
			string description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
			string company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
			string product = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
			string copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
			string trademark = assembly.GetCustomAttribute<AssemblyTrademarkAttribute>().Trademark;

			string versionS;
			AssemblyVersionAttribute assemblyVersion = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
			if(assemblyVersion != null) {
				versionS = assemblyVersion.Version;
			} else {
				AssemblyFileVersionAttribute fileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
				if(fileVersion == null) {
					throw new Exception("Assembly version could not be found.");
				}
				versionS = fileVersion.Version;
			}
			Version version = Version.Parse(versionS);

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
		/// Gets the method that called the current method where this method is used.
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
		/// Gets the types defined in this assembly and in the specified namespace.
		/// </summary>
		/// <param name="assembly">The assembly from which to get the types.</param>
		/// <param name="namespace">The namespace in which the types are defined.</param>
		public static IEnumerable<Type> GetTypesInNamespace(this Assembly assembly, string @namespace)
		{
			return assembly.GetTypes().Where(t => t.Namespace == @namespace);
		}

		/// <summary>
		/// Gets the value of the specified property in the object.
		/// </summary>
		/// <param name="obj">The object that has the property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static object GetPropertyValue(this object obj, string propertyName)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName);
			return property.GetValue(obj);
		}

		/// <summary>
		/// Gets the type of the specified property in the type.
		/// <para>
		/// If the type is nullable, this function gets its generic definition. To get the real type, use <see cref="GetPropertyTypeReal(Type, string)"/>.
		/// </para>
		/// </summary>
		/// <param name="type">The type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static Type GetPropertyType(this Type type, string propertyName)
		{
			Type propertyType = GetPropertyTypeReal(type, propertyName);

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
		public static Type GetPropertyTypeReal(this Type type, string propertyName)
		{
			PropertyInfo property = GetPropertyInfo(type, propertyName);
			return property.PropertyType;
		}

		/// <summary>
		/// Gets the property information by name for the type of the object.
		/// </summary>
		/// <param name="obj">Object with a type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static PropertyInfo GetPropertyInfo(this object obj, string propertyName)
		{
			return GetPropertyInfo(obj.GetType(), propertyName);
		}

		/// <summary>
		/// Gets the property information by name for the type.
		/// </summary>
		/// <param name="type">Type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
		{
			PropertyInfo property = type.GetProperty(propertyName);
			if(property == null) {
				throw new Exception(string.Format("The provided property name ({0}) does not exist in type '{1}'.", propertyName, type.ToString()));
			}

			return property;
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
		/// </summary>
		/// <param name="obj">The object with the property.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="value">The value to set the property to.</param>
		public static void SetProperty(this object obj, string propertyName, object value)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName);
			property.SetValue(obj, value);
		}

		/// <summary>
		/// Sets the specified property to a value that will be extracted from the provided string value using the <see cref="TypeDescriptor.GetConverter(Type)"/> and <see cref="TypeConverter.ConvertFromString(string)"/>.
		/// </summary>
		/// <param name="obj">The object with the property.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="valueAsString">The string representation of the value to set to the property.</param>
		public static void SetPropertyFromString(this object obj, string propertyName, string valueAsString)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName);
			TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);
			object value = converter.ConvertFromString(valueAsString);
			property.SetValue(obj, value);
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
		/// <para><see cref="string"/> is considered a primitive.</para>
		/// </summary>
		/// <param name="type">The type.</param>
		public static bool IsPrimitive(this Type type)
		{
			if(type == typeof(string)) {
				// string is considered as a primitive
				return true;
			}

			return type.IsValueType && type.IsPrimitive;
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
			return default(T);
		}
	}
}
