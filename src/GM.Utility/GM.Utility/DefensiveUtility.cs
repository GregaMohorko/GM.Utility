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
Created: 2022-11-5
Author: Gregor Mohorko
*/

using System.Collections.Generic;
using System;
using System.Linq;

namespace GM.Utility;
/// <summary>
/// Defensive utilities.
/// </summary>
public static class DefensiveUtility
{
	/// <summary>
	/// Throws <see cref="ArgumentException"/> if <paramref name="value"/> is false.
	/// </summary>
	/// <param name="value">The bool value.</param>
	/// <param name="message">The exception message.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <exception cref="ArgumentException" />
	public static void ThrowIfFalse(this bool value, string message = null, string paramName = null)
	{
		if(!value) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentException(p, m),
				exceptionMsg: p => new ArgumentException(p),
				exceptionParam: m => new ArgumentException(m, innerException: null),
				exception: () => new ArgumentException()
			);
		}
	}

	/// <summary>
	/// Throws <see cref="InvalidOperationException"/> if <paramref name="value"/> is false.
	/// </summary>
	/// <param name="value">The bool value.</param>
	/// <param name="message">The exception message.</param>
	/// <exception cref="InvalidOperationException" />
	public static void ThrowIfFalseState(this bool value, string message = null)
	{
		if(!value) {
			ThrowException(
				paramName: null,
				message,
				exceptionMsgParam: (p, m) => new InvalidOperationException($"{m} Parameter name: {p}"),
				exceptionMsg: m => new InvalidOperationException(m),
				exceptionParam: _ => null,
				exception: () => new InvalidOperationException()
			);
		}
	}

	/// <summary>
	/// Throws <see cref="ArgumentNullException"/> if <paramref name="obj"/> is null.
	/// </summary>
	/// <typeparam name="T">Object type.</typeparam>
	/// <param name="obj">This object.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same object.</returns>
	/// <exception cref="ArgumentNullException" />
	public static T ThrowIfNull<T>(this T obj, string paramName = null, string message = null)
		where T : class
	{
		if(obj == null) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentNullException(p, m),
				exceptionMsg: m => new ArgumentNullException(m, innerException: null),
				exceptionParam: p => new ArgumentNullException(p),
				exception: () => new ArgumentNullException()
			);
		}

		return obj;
	}

	/// <summary>
	/// Throws <see cref="ArgumentNullException"/> if <paramref name="obj"/> is null.
	/// </summary>
	/// <typeparam name="T">Object type.</typeparam>
	/// <param name="obj">This object.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same object.</returns>
	/// <exception cref="ArgumentNullException" />
	public static T ThrowIfNull<T>(this T? obj, string paramName = null, string message = null)
		where T : struct
	{
		if(!obj.HasValue) {
			throw new ArgumentNullException(paramName, message);
		}

		return obj.Value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentException"/> if <paramref name="list"/> is null or <see cref="ArgumentException"/> if it is empty.
	/// </summary>
	/// <typeparam name="T">Type of elements.</typeparam>
	/// <param name="list">This collection.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same collection.</returns>
	/// <exception cref="ArgumentNullException" />
	/// <exception cref="ArgumentException" />
	public static ICollection<T> ThrowIfNullOrEmpty<T>(this ICollection<T> list, string paramName = null, string message = null)
	{
		ThrowIfNull(list, paramName, message);
		if(!list.Any()) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentException(m, p),
				exceptionMsg: p => new ArgumentException(message: null, p),
				exceptionParam: m => new ArgumentException(m),
				exception: () => new ArgumentException()
			);
		}

		return list;
	}

	/// <summary>
	/// Throws <see cref="ArgumentException"/> if <paramref name="s"/> is null or <see cref="ArgumentException"/> if it is empty.
	/// </summary>
	/// <param name="s">This string.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same string.</returns>
	/// <exception cref="ArgumentNullException" />
	/// <exception cref="ArgumentException" />
	public static string ThrowIfNullOrEmpty(this string s, string paramName = null, string message = null)
	{
		ThrowIfNull(s, paramName, message);
		if(s == string.Empty) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentException(m, p),
				exceptionMsg: p => new ArgumentException(message: null, p),
				exceptionParam: m => new ArgumentException(m),
				exception: () => new ArgumentException()
			);
		}

		return s;
	}

	/// <summary>
	/// Throws <see cref="ArgumentOutOfRangeException"/> if this value is out of the specified min/max range.
	/// </summary>
	/// <typeparam name="T">The type of value.</typeparam>
	/// <param name="value">This value.</param>
	/// <param name="min">The inclusive min value.</param>
	/// <param name="max">The inclusive max value.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same value.</returns>
	/// <exception cref="ArgumentOutOfRangeException" />
	public static T ThrowIfOutOfRange<T>(this T value, T min, T max, string paramName = null, string message = null)
		where T : IComparable<T>
	{
		if(value.CompareTo(min) < 0 || value.CompareTo(max) > 0) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentOutOfRangeException(p, m),
				exceptionMsg: p => new ArgumentOutOfRangeException(p),
				exceptionParam: m => new ArgumentOutOfRangeException(m, innerException: null),
				exception: () => new ArgumentOutOfRangeException()
			);
		}

		return value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentOutOfRangeException"/> if this value is less than the specified min value.
	/// </summary>
	/// <typeparam name="T">The type of value.</typeparam>
	/// <param name="value">This value.</param>
	/// <param name="min">The inclusive min value.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same value.</returns>
	/// <exception cref="ArgumentOutOfRangeException" />
	public static T ThrowIfLessThan<T>(this T value, T min, string paramName = null, string message = null)
		where T : IComparable<T>
	{
		if(value.CompareTo(min) < 0) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentOutOfRangeException(p, m),
				exceptionMsg: p => new ArgumentOutOfRangeException(p),
				exceptionParam: m => new ArgumentOutOfRangeException(m, innerException: null),
				exception: () => new ArgumentOutOfRangeException()
			);
		}

		return value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentOutOfRangeException"/> if this value is less or equal to the specified min value.
	/// </summary>
	/// <typeparam name="T">The type of value.</typeparam>
	/// <param name="value">This value.</param>
	/// <param name="min">The exclusive min value.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same value.</returns>
	/// <exception cref="ArgumentOutOfRangeException" />
	public static T ThrowIfLessOrEqualThan<T>(this T value, T min, string paramName = null, string message = null) where T : IComparable<T>
	{
		if(value.CompareTo(min) <= 0) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentOutOfRangeException(p, m),
				exceptionMsg: p => new ArgumentOutOfRangeException(p),
				exceptionParam: m => new ArgumentOutOfRangeException(m, innerException: null),
				exception: () => new ArgumentOutOfRangeException()
			);
		}

		return value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentOutOfRangeException"/> if this value is not equal to the expected value.
	/// </summary>
	/// <typeparam name="T">The type of value.</typeparam>
	/// <param name="value">This value.</param>
	/// <param name="expectedValue">Expected value.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same value.</returns>
	/// <exception cref="ArgumentOutOfRangeException" />
	public static T ThrowIfNotEqual<T>(this T value, T expectedValue, string paramName = null, string message = null)
		where T : IComparable<T>
	{
		if(value.CompareTo(expectedValue) != 0) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentOutOfRangeException(p, m),
				exceptionMsg: p => new ArgumentOutOfRangeException(p),
				exceptionParam: m => new ArgumentOutOfRangeException(m, innerException: null),
				exception: () => new ArgumentOutOfRangeException()
			);
		}

		return value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentOutOfRangeException"/> if this value is greater than the specified min value.
	/// </summary>
	/// <typeparam name="T">The type of value.</typeparam>
	/// <param name="value">This value.</param>
	/// <param name="max">The inclusive max value.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same value.</returns>
	/// <exception cref="ArgumentOutOfRangeException" />
	public static T ThrowIfGreaterThan<T>(this T value, T max, string paramName = null, string message = null) where T : IComparable<T>
	{
		if(value.CompareTo(max) > 0) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentOutOfRangeException(p, m),
				exceptionMsg: p => new ArgumentOutOfRangeException(p),
				exceptionParam: m => new ArgumentOutOfRangeException(m, innerException: null),
				exception: () => new ArgumentOutOfRangeException()
			);
		}

		return value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentOutOfRangeException"/> if this value is greater or equal to the specified min value.
	/// </summary>
	/// <typeparam name="T">The type of value.</typeparam>
	/// <param name="value">This value.</param>
	/// <param name="max">The exclusive max value.</param>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="message">The exception message.</param>
	/// <returns>The same value.</returns>
	/// <exception cref="ArgumentOutOfRangeException" />
	public static T ThrowIfGreaterOrEqualThan<T>(this T value, T max, string paramName = null, string message = null) where T : IComparable<T>
	{
		if(value.CompareTo(max) >= 0) {
			ThrowException(
				paramName,
				message,
				exceptionMsgParam: (p, m) => new ArgumentOutOfRangeException(p, m),
				exceptionMsg: p => new ArgumentOutOfRangeException(p),
				exceptionParam: m => new ArgumentOutOfRangeException(m, innerException: null),
				exception: () => new ArgumentOutOfRangeException()
			);
		}

		return value;
	}

	/// <summary>
	/// Throws <see cref="ArgumentException"/> if the specified item is not in this collection.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <param name="source">The collection.</param>
	/// <param name="item">The item.</param>
	/// <exception cref="ArgumentException"></exception>
	public static void ThrowIfNotContained<T>(this IEnumerable<T> source, T item)
	{
		if(!source.Contains(item)) {
			throw new ArgumentException($"The item {item} was expected to be contained in the collection.");
		}
	}

	private static void ThrowException<TException>(
		string paramName,
		string message,
		Func<string, string, TException> exceptionMsgParam,
		Func<string, TException> exceptionMsg = null,
		Func<string, TException> exceptionParam = null,
		Func<TException> exception = null
		) where TException : Exception
	{
		if(string.IsNullOrWhiteSpace(paramName)) {
			if(string.IsNullOrWhiteSpace(message)) {
				throw exception?.Invoke() ??
					  exceptionMsg?.Invoke(string.Empty) ??
					  exceptionParam?.Invoke(string.Empty) ??
					  exceptionMsgParam?.Invoke(string.Empty, string.Empty);
			}
			throw exceptionMsg?.Invoke(message) ??
				  exceptionMsgParam?.Invoke(message, string.Empty) ??
				  exceptionParam?.Invoke(string.Empty) ??
				  exception?.Invoke();
		}
		if(string.IsNullOrWhiteSpace(message)) {
			throw exceptionParam?.Invoke(paramName) ??
				  exceptionMsgParam?.Invoke(string.Empty, paramName) ??
				  exception?.Invoke() ??
				  exceptionMsg?.Invoke(string.Empty);
		}
		throw exceptionMsgParam?.Invoke(message, paramName) ??
			  exceptionParam?.Invoke(paramName) ??
			  exceptionMsg?.Invoke(message) ??
			  exception?.Invoke();
	}
}
