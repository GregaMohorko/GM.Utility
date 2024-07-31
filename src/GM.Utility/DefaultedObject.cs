using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// An object whose fields and properties of the specified type are set to a predefined default value.
	/// <para>Internal, protected and private fields are included, static are not.</para>
	/// </summary>
	/// <typeparam name="T">The type of the properties that will be set to a default value.</typeparam>
    public abstract class DefaultedObject<T>
    {
		/// <summary>
		/// Will get all fields and properties and set them to the value returned by <see cref="Default"/>.
		/// </summary>
		protected DefaultedObject()
		{
			ReflectionUtility.SetAllPropertiesOfType(this, Default);
		}

		/// <summary>
		/// Gets the value to which all fields of the specified type will be set on object initialization.
		/// </summary>
		protected abstract T Default { get; }
    }
}
