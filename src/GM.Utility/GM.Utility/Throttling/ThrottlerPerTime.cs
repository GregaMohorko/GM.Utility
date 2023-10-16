/*
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
Created: 2023-10-16
Author: Gregor Mohorko
*/

using System;
using System.Threading;
using System.Threading.Tasks;

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
	/// The max number of executions that are allowed in the time frame set in <see cref="Time"/>.
	/// </summary>
	public int MaxExecutions { get; }

	private volatile int _executionLogPosition;
	private readonly DateTime[] _executionLog;

	/// <summary>
	/// Creates a new instance of <see cref="ThrottlerPerTime"/>.
	/// </summary>
	/// <param name="time">The time frame.</param>
	/// <param name="maxExecutions">The max number of executions that are allowed in the specified time frame.</param>
	public ThrottlerPerTime(TimeSpan time, int maxExecutions)
	{
		Time = time;
		MaxExecutions = maxExecutions;

		_executionLog = new DateTime[maxExecutions];
		_executionLogPosition = 0;
	}

	/// <summary>
	/// Checks the number of executions in the 
	/// <para>Thread-safe.</para>
	/// </summary>
	/// <param name="ct">Cancellation token.</param>
	public async Task WaitExecutionLimit(CancellationToken ct)
	{
		while(true) {
			DateTime utcNow = DateTime.UtcNow;
			DateTime nextAvailableExecutionAt;

			lock(_executionLog) {
				nextAvailableExecutionAt = _executionLog[_executionLogPosition].Add(Time);
				if(utcNow >= nextAvailableExecutionAt) {
					// can execute now

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
			await Task.Delay(sleepTime, ct);
		}
	}
}
