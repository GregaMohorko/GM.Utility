using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for checking equality of objects.
	/// </summary>
	public static class EqualityUtility
	{
		/// <summary>
		/// Determines whether these two values are considered equal (using a default comparer).
		/// </summary>
		/// <typeparam name="T">The type of the objects.</typeparam>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		public static bool AreEqual<T>(T x, T y)
		{
			return EqualityComparer<T>.Default.Equals(x, y);
		}

		/// <summary>
		/// Determines whether this value is equal to the default value of the type <typeparamref name="T"/> (using a default comparer).
		/// </summary>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <param name="obj">The object to compare to the default value.</param>
		public static bool IsDefault<T>(this T obj)
		{
			return EqualityComparer<T>.Default.Equals(obj, default(T));
		}
	}
}
