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

Project: GM.Utility.Testing.Unit
Created: 2024-7-31
Author: grega
*/

using GM.Utility.Throttling;

namespace GM.Utility.Testing.Unit.Throttling;
public class ThrottlerPerTimeTests
{
	[Fact]
	public async Task WaitExecutionLimit()
	{
		var throttler = new ThrottlerPerTime(
			limits: new[]
			{
				// max 8 in the span of 200 ms
				(TimeSpan.FromMilliseconds(200), 8),
				// max 5 in the span of 50 ms
				(TimeSpan.FromMilliseconds(50), 5)
			}
			);

		const int EXECUTION_TARGET = 16;
		int executionCount = 0;

		var executingThread = new Thread(async () =>
		{
			for(int i = EXECUTION_TARGET; i > 0; --i) {
				await throttler.WaitExecutionLimit(CancellationToken.None);
				++executionCount;
			}
		});

		executingThread.Start();

		await Task.Delay(30);
		executionCount.Should().Be(5);

		await Task.Delay(150 - 30);
		executionCount.Should().Be(8);

		await Task.Delay(230 - 150);
		executionCount.Should().Be(8 + 5);

		await Task.Delay(300 - 230);
		executionCount.Should().Be(EXECUTION_TARGET);
	}
}
