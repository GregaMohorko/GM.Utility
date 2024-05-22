using System;

namespace GM.Utility;
/// <summary>
/// Utilities for <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanUtility
{
	/// <summary>
	/// Returns the maximum value of the two provided values.
	/// </summary>
	/// <param name="val1">The first value.</param>
	/// <param name="val2">The second value.</param>
	public static TimeSpan Max(TimeSpan val1, TimeSpan val2)
	{
		return val1 > val2 ? val1 : val2;
	}
}
