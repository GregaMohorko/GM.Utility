﻿/*
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

Project: GM.Utility
Created: 2023-10-16
Author: Gregor Mohorko
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GM.Utility.Throttling;
/// <summary>
/// Allows you to specify the number of executions per time-frame and call this throttler to wait before each execution and thus insure that the limit is respected.
/// <para>Thread-safe.</para>
/// </summary>
public class ThrottlerPerTime
{
	/// <summary>
	/// The time frame of this throttler.
	/// </summary>
	public TimeSpan Time { get; }
	/// <summary>
	/// The max number of executions that are allowed in the time frame set in <see cref="Time"/>. Zero means no limit.
	/// </summary>
	public int MaxExecutions { get; }

	private volatile int _executionLogPosition;
	private readonly DateTime[] _executionLog;

	private readonly ILogger _logger;

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="time">The time frame.</param>
	/// <param name="maxExecutions">The max number of executions that are allowed in the specified time frame. Zero means no limit.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when max executions is not non-negative.</exception>
	public ThrottlerPerTime(TimeSpan time, int maxExecutions)
		: this(time, maxExecutions, null)
	{ }

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="time">The time frame.</param>
	/// <param name="maxExecutions">The max number of executions that are allowed in the specified time frame. Zero means no limit.</param>
	/// <param name="logger">Logger.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when max executions is not non-negative.</exception>
	public ThrottlerPerTime(TimeSpan time, int maxExecutions, ILogger logger)
	{
		if(maxExecutions < 0) {
			throw new ArgumentOutOfRangeException(nameof(maxExecutions), maxExecutions, "Should be non-negative.");
		}

		_logger = logger;

		Time = time;
		MaxExecutions = maxExecutions;

		_executionLog = new DateTime[maxExecutions];
		_executionLogPosition = 0;
	}

	/// <summary>
	/// Waits until the next time there can be an execution, according to the set throttling settings.
	/// <para>Thread-safe.</para>
	/// </summary>
	public async Task WaitExecutionLimit(CancellationToken ct)
	{
		if(MaxExecutions == 0) {
			// no limit
			return;
		}

		while(true) {
			DateTime utcNow = DateTime.UtcNow;
			DateTime nextAvailableExecutionAt;

			lock(_executionLog) {
				nextAvailableExecutionAt = _executionLog[_executionLogPosition].Add(Time);
				if(utcNow >= nextAvailableExecutionAt) {
					// can execute now
					_logger?.LogDebug("Position: {current}/{total}. Can execute now, since NextAvailableExecutionAt={nextAvailableExecutionAt} <= UtcNow={utcNow}.", _executionLogPosition, _executionLog.Length, nextAvailableExecutionAt, utcNow);

					// log new execution
					_executionLog[_executionLogPosition] = DateTime.UtcNow;
					_executionLogPosition = (_executionLogPosition + 1) % _executionLog.Length;

					return;
				} else {
					// needs to wait
				}
			}

			// wait and try again at next available execution
			var sleepTime = nextAvailableExecutionAt - utcNow;
			_logger?.LogDebug("Position: {current}/{total}. Cannot execute now, since NextAvailableExecutionAt={nextAvailableExecutionAt} > UtcNow={utcNow}. Sleeping for {sleepTime}.", _executionLogPosition, _executionLog.Length, nextAvailableExecutionAt, utcNow, sleepTime);
			await Task.Delay(sleepTime, ct);
		}
	}
}
