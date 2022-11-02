/*
MIT License

Copyright (c) 2022 Gregor Mohorko

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
Created: 2022-11-2
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GM.Utility.Framework;

/// <summary>
/// Base class for all enumerations (https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types).
/// </summary>
public abstract class Enumeration<TId> : IComparable where TId : IComparable
{
	/// <summary>
	/// Name of the enumeration.
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// The enum of the enumeration, serving as an ID.
	/// </summary>
	public TId Id { get; }

	/// <summary>
	/// Creates a new instance of <see cref="Enumeration{TId}"/>.
	/// </summary>
	/// <param name="id">The ID.</param>
	/// <param name="name">The name.</param>
	protected Enumeration(TId id, string name)
	{
		Id = id;
		Name = name;
	}

	/// <inheritdoc/>
	public override string ToString() => Name;

	/// <summary>
	/// Returns all enumeration instances of the specified enum ID.
	/// </summary>
	/// <typeparam name="T">The enumeration type.</typeparam>
	public static IEnumerable<T> GetAll<T>() where T : Enumeration<TId>
	{
		var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
		var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

		return fields.Select(f => f.GetValue(null)).Union(properties.Select(p => p.GetValue(null))).Cast<T>();
	}

	/// <inheritdoc/>
	public override bool Equals(object obj)
	{
		if(obj is not Enumeration<TId> otherValue) {
			return false;
		}

		var thisType = GetType();
		var objType = obj.GetType();

		var typeMatches = false;
		if(thisType.IsAssignableFrom(objType)
			|| objType.IsAssignableFrom(thisType)
			) {
			typeMatches = true;
		}

		var valueMatches = Id.Equals(otherValue.Id);

		return typeMatches && valueMatches;
	}

	/// <inheritdoc/>
	public override int GetHashCode() => Id.GetHashCode();

	/// <summary>
	/// Returns the enumeration instance of the specified ID.
	/// </summary>
	/// <typeparam name="T">The enumeration type.</typeparam>
	/// <param name="value">The ID enum value.</param>
	/// <param name="comparison">By default, uses <see cref="CompareTo(object)"/>.</param>
	public static T FromValue<T>(TId value, Func<TId, TId, bool> comparison = null) where T : Enumeration<TId>
	{
		comparison ??= (a, b) => a.CompareTo(b) == 0;
		var matchingItem = Parse<T, TId>(value, "value", item => comparison(item.Id, value));
		return matchingItem;
	}

	/// <summary>
	/// Returns the enumeration instance with the specified name.
	/// </summary>
	/// <typeparam name="T">The enumeration type.</typeparam>
	/// <param name="displayName">The name.</param>
	/// <param name="stringComparison">String comparison.</param>
	public static T FromDisplayName<T>(string displayName, StringComparison stringComparison = StringComparison.InvariantCulture) where T : Enumeration<TId>
	{
		var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name.Equals(displayName, stringComparison));
		return matchingItem;
	}

	private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration<TId>
	{
		var matchingItem = GetAll<T>().FirstOrDefault(predicate);

		if(matchingItem == null)
			throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

		return matchingItem;
	}

	/// <summary>
	/// Compares this value to the other value.
	/// </summary>
	/// <param name="other">The other value.</param>
	public int CompareTo(object other) => Id.CompareTo(((Enumeration<TId>)other).Id);
}
