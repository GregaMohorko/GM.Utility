using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class ReflectionUtility
	{
		/// <summary>
		/// Specifies the type of the assembly.
		/// </summary>
		public enum AssemblyType
		{
			Unknown = 0,
			/// <summary>
			/// Represents the entry assembly, which is usually the assembly of the application.
			/// </summary>
			Application = 1,
			/// <summary>
			/// Represents the assembly of the library where this enum is defined.
			/// </summary>
			Library = 2,
			/// <summary>
			/// Represents the currently running assembly.
			/// </summary>
			Current = 3
		}

		/// <summary>
		/// Structure with assembly information.
		/// </summary>
		public struct AssemblyInformation
		{
			public readonly string Title;
			public readonly string Description;
			public readonly string Company;
			public readonly string Product;
			public readonly string Copyright;
			public readonly string Trademark;
			public readonly Version Version;

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
				case AssemblyType.Application:
					assembly = Assembly.GetEntryAssembly();
					break;
				case AssemblyType.Library:
					assembly = Assembly.GetExecutingAssembly();
					break;
				case AssemblyType.Current:
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
		/// AssemblyType.Current is not allowed.
		/// </para>
		/// </summary>
		/// <param name="assemblyType">The type of the assembly to look for.</param>
		public static AssemblyInformation GetAssemblyInformation(AssemblyType assemblyType)
		{
			if(assemblyType == AssemblyType.Current || assemblyType==AssemblyType.Application) {
				throw new InvalidEnumArgumentException("AssemblyType.Current and AssemblyType.Application are not allowed when getting the assembly information. Use GetAssembly() to get the assembly first, then call GetAssemblyInformation(Assembly) with it.");
			}
			var assembly = GetAssembly(assemblyType);
			return GetAssemblyInformation(assembly);
		}

		/// <summary>
		/// Gets the assembly information of the specified type.
		/// </summary>
		/// <param name="assembly">The assembly from which to extract the information from.</param>
		public static AssemblyInformation GetAssemblyInformation(Assembly assembly)
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
		public static string GetAssemblyDirectoryLocalPath(Type type)
		{
			return GetAssemblyDirectoryLocalPath(type.Assembly);
		}

		/// <summary>
		/// Gets the local (operating-system wise) directory path of the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static string GetAssemblyDirectoryLocalPath(Assembly assembly)
		{
			string filePath = GetAssemblyFileLocalPath(assembly);
			return Path.GetDirectoryName(filePath);
		}

		/// <summary>
		/// Gets the local (operating-system wise) file (.dll) path of the assembly that defines the specified type.
		/// </summary>
		/// <param name="type">The type that is defined in the assembly of which to get the local file path.</param>
		public static string GetAssemblyFileLocalPath(Type type)
		{
			return GetAssemblyFileLocalPath(type.Assembly);
		}

		/// <summary>
		/// Gets the local (operating-system wise) file (.dll) path of the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		public static string GetAssemblyFileLocalPath(Assembly assembly)
		{
			string codebase = assembly.CodeBase;
			var uri = new Uri(codebase, UriKind.Absolute);
			return uri.LocalPath;
		}

		/// <summary>
		/// Sets the specified property to the provided value in the provided object.
		/// </summary>
		/// <param name="obj">The object with the property.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="value">The value to set the property to.</param>
		public static void SetProperty(object obj, string propertyName, object value)
		{
			GetPropertyInfo(obj, propertyName).SetValue(obj, value);
		}

		public static void SetProperty(object obj, string propertyName, string valueAsString)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName);
			property.SetValue(obj, TypeDescriptor.GetConverter(property.PropertyType).ConvertFromString(valueAsString));
		}

		/// <summary>
		/// Gets the value the specified property in the provided object.
		/// </summary>
		/// <param name="obj">The object that has the property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static object GetPropertyValue(object obj, string propertyName)
		{
			PropertyInfo property = GetPropertyInfo(obj, propertyName);

			return property.GetValue(obj);
		}

		/// <summary>
		/// Gets the type of the specified property in the specified type.
		/// <para>
		/// If the type is nullable, this function gets its generic definition.
		/// </para>
		/// </summary>
		/// <param name="type">The type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static Type GetPropertyType(Type type, string propertyName)
		{
			PropertyInfo property = GetPropertyInfo(type, propertyName);

			Type propertyType = property.PropertyType;

			// get the generic type of nullable, not THE nullable
			if(propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				propertyType = GetGenericFirst(propertyType);

			return propertyType;
		}

		/// <summary>
		/// Gets the property information by name for the type of the provided object.
		/// </summary>
		/// <param name="obj">Object with a type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static PropertyInfo GetPropertyInfo(object obj, string propertyName)
		{
			return GetPropertyInfo(obj.GetType(), propertyName);
		}

		/// <summary>
		/// Gets the property information by name for the specified type.
		/// </summary>
		/// <param name="type">Type that has the specified property.</param>
		/// <param name="propertyName">The name of the property.</param>
		public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
		{
			PropertyInfo property = type.GetProperty(propertyName);
			if(property == null)
				throw new Exception(string.Format("The provided property name ({0}) does not exist in type '{1}'.", propertyName, type.ToString()));

			return property;
		}

		/// <summary>
		/// Returns the first generic type of the specified type.
		/// </summary>
		/// <param name="type">The type from which to get the generic type.</param>
		public static Type GetGenericFirst(Type type)
		{
			return type.GetGenericArguments()[0];
		}

		public static List<FieldInfo> GetConstants(Type type, bool includeInherited)
		{
			FieldInfo[] fields;

			if(includeInherited)
				fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			else
				fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			return fields.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
		}

		public static bool IsPropertyOf(object obj, string propertyName, bool includeInherited)
		{
			Type type = obj.GetType();

			return IsPropertyOf(type, propertyName, includeInherited);
		}

		public static bool IsPropertyOf(Type type, string propertyName, bool includeInherited)
		{
			PropertyInfo propertyInfo;

			if(includeInherited)
				propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			else
				propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

			return propertyInfo != null;
		}

		/// <summary>
		/// Determines whether the specified type to check derives from the specified generic type.
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
			if(obj == null)
				return false;
			return IsGenericList(obj.GetType());
		}

		/// <summary>
		/// Determines whether the specified type is a generic list.
		/// </summary>
		/// <param name="type">The type to determine.</param>
		public static bool IsGenericList(Type type)
		{
			if(!typeof(IList).IsAssignableFrom(type))
				return false;
			if(!type.IsGenericType)
				return false;
			if(!type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
				return false;
			return true;
		}
	}
}
