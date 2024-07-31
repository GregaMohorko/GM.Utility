/*
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
using System.Linq;
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
	/// A collection of common limits of this throttler. The Time is the time frame of the specific limit. The MaxExecutions is the max number of executions that are allowed in the time frame (zero means no limit).
	/// </summary>
	public (TimeSpan Time, int MaxExecutions)[] Limits { get; }

	private volatile int _executionLogPosition;
	private readonly DateTime[] _executionLog;

	private readonly ILogger _logger;

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="time">The time frame.</param>
	/// <param name="maxExecutions">The max number of executions that are allowed in the specified time frame. Zero means no limit.</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	/// <exception cref="ArgumentException">Thrown when max executions is not non-negative.</exception>
	public ThrottlerPerTime(TimeSpan time, int maxExecutions)
		: this(time, maxExecutions, null)
	{ }

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="time">The time frame.</param>
	/// <param name="maxExecutions">The max number of executions that are allowed in the specified time frame. Zero means no limit.</param>
	/// <param name="logger">Logger.</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	/// <exception cref="ArgumentException">Thrown when max executions is not non-negative.</exception>
	public ThrottlerPerTime(TimeSpan time, int maxExecutions, ILogger logger)
		: this(new[] { (time, maxExecutions) }, logger)
	{ }

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="limits">A collection of limits for this throttler. A single limit consists of the time frame and the max number of executions that are allowed in this time frame. Zero means no limit.</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	/// <exception cref="ArgumentException">Thrown when a max executions of any limit is not non-negative.</exception>
	public ThrottlerPerTime((TimeSpan Time, int MaxExecutions)[] limits)
		: this(limits, null)
	{ }

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="limits">A collection of limits for this throttler. A single limit consists of the time frame and the max number of executions that are allowed in this time frame. Zero means no limit.</param>
	/// <param name="logger">Logger.</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	/// <exception cref="ArgumentException">Thrown when a max executions of any limit is not non-negative.</exception>
	public ThrottlerPerTime((TimeSpan Time, int MaxExecutions)[] limits, ILogger logger)
	{
		if(limits == null) {
			throw new ArgumentNullException(nameof(limits));
		}
		if(limits.Length == 0) {
			throw new ArgumentOutOfRangeException(nameof(limits), "Empty.");
		}
		if(limits.Any(l => l.MaxExecutions < 0)) {
			var (_, MaxExecutions) = limits[0];
			throw new ArgumentException($"{nameof(MaxExecutions)} should be non-negative.", nameof(limits));
		}

		_logger = logger;

		Limits = limits;

		int maxExecutions = limits.Max(l => l.MaxExecutions);
		_executionLog = new DateTime[maxExecutions];
		_executionLogPosition = 0;
	}

	/// <summary>
	/// Waits until the next time there can be an execution, according to the set throttling settings.
	/// <para>Thread-safe.</para>
	/// </summary>
	public async Task WaitExecutionLimit(CancellationToken ct)
	{
		if(Limits.All(l => l.MaxExecutions == 0)) {
			// no limit
			return;
		}

		while(true) {
			DateTime utcNow = DateTime.UtcNow;
			DateTime nextAvailableExecutionAt = DateTime.MinValue;

			lock(_executionLog) {
				foreach(var (Time, MaxExecutions) in Limits) {
					if(MaxExecutions == 0) {
						// no limit
						continue;
					}
					int previousRelevantLogPosition = (_executionLogPosition - MaxExecutions + _executionLog.Length) % _executionLog.Length;
					DateTime previousExecutionTime = _executionLog[previousRelevantLogPosition];
					DateTime limitsNextAvailableExecutionAt = previousExecutionTime.Add(Time);
					if(limitsNextAvailableExecutionAt > nextAvailableExecutionAt) {
						nextAvailableExecutionAt = limitsNextAvailableExecutionAt;
					}
				}

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
			TimeSpan sleepTime = nextAvailableExecutionAt - utcNow;
			_logger?.LogDebug("Position: {current}/{total}. Cannot execute now, since NextAvailableExecutionAt={nextAvailableExecutionAt} > UtcNow={utcNow}. Sleeping for {sleepTime}.", _executionLogPosition, _executionLog.Length, nextAvailableExecutionAt, utcNow, sleepTime);
			await Task.Delay(sleepTime, ct);
		}
	}
}
