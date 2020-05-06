/*
MIT License

Copyright (c) 2020 Gregor Mohorko

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
Created: 2020-04-04
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="Task"/>.
	/// </summary>
	public static class TaskUtility
	{
		/// <summary>
		/// Creates a task that will complete either when the provided task is completed or immediately on cancellation request. This method returns true if the task was cancelled.
		/// <para>This method allows you to create a task that returns immediately on cancellation request, before the task itself finishes. Note that the task itself will run until the end even if the cancellation was requested.</para>
		/// </summary>
		/// <param name="task">The task to wait on for completion.</param>
		/// <param name="ct">The cancellation token with which this task can be immediately canceled.</param>
		public static async Task<bool> AwaitOrCancel(Task task, CancellationToken ct)
		{
			// https://johnthiriet.com/cancel-asynchronous-operation-in-csharp

			if(task == null) {
				throw new ArgumentNullException(nameof(task));
			}
			if(ct == null) {
				throw new ArgumentNullException(nameof(ct));
			}

			var taskComplectionSource = new TaskCompletionSource<object>();

			_ = ct.Register(delegate
			{
				_ = taskComplectionSource.TrySetCanceled();
			});

			Task completedTask = await Task.WhenAny(task, taskComplectionSource.Task);

			try {
				// the task is finished and the await will return immediately (the point of this is to throw exception if it occured)
				await completedTask;
			} catch(OperationCanceledException) {
				// do nothing ...
			}

			return ct.IsCancellationRequested;
		}

		/// <summary>
		/// Creates a task that will complete when the provided task is completed or it will throw <see cref="OperationCanceledException"/> immediately on cancellation request.
		/// <para>This method allows you to create a task that throws <see cref="OperationCanceledException"/> immediately on cancellation request, before the task itself finishes. Note that the task itself will run until the end even if the cancellation was requested.</para>
		/// </summary>
		/// <param name="task">The task to wait on for completion.</param>
		/// <param name="ct">The cancellation token with which this task can be immediately canceled.</param>
		/// <exception cref="OperationCanceledException" />
		public static async Task AwaitOrCancelThrow(Task task, CancellationToken ct)
		{
			_ = await AwaitOrCancel(task, ct);
			ct.ThrowIfCancellationRequested();
		}

		/// <summary>
		/// Queues the specified work to run on the thread pool and creates a task that will complete either when the provided work is completed or immediately on cancellation request. This method returns true if the task was cancelled.
		/// <para>This method allows you to create a task that returns immediately on cancellation request, before the task itself finishes. Note that the work itself will run until the end even if the cancellation was requested.</para>
		/// </summary>
		/// <param name="action">The work to execute asynchronously.</param>
		/// <param name="ct">The cancellation token with which this task can be immediately canceled.</param>
		public static async Task<bool> RunOrCancel(Action action, CancellationToken ct)
		{
			return await AwaitOrCancel(Task.Run(action), ct);
		}

		/// <summary>
		/// Queues the specified work to run on the thread pool and creates a task that will complete when the provided work is completed or it will throw <see cref="OperationCanceledException"/> immediately on cancellation request.
		/// <para>This method allows you to create a task that throws <see cref="OperationCanceledException"/> immediately on cancellation request, before the task itself finishes. Note that the work itself will run until the end even if the cancellation was requested.</para>
		/// </summary>
		/// <param name="action">The work to execute asynchronously.</param>
		/// <param name="ct">The cancellation token with which this task can be immediately canceled.</param>
		/// <exception cref="OperationCanceledException" />
		public static async Task RunOrCancelThrow(Action action, CancellationToken ct)
		{
			_ = await RunOrCancel(action, ct);
			ct.ThrowIfCancellationRequested();
		}

		/// <summary>
		/// Creates a new thread with <see cref="ApartmentState.STA"/> apartment state that runs the the specified work and returns a <see cref="Task"/> that represents that work.
		/// </summary>
		/// <typeparam name="TResult">The return type of the task.</typeparam>
		/// <param name="function">The work to execute asynchronously.</param>
		public static Task<TResult> RunSTA<TResult>(Func<TResult> function)
		{
			if(function == null) {
				throw new ArgumentNullException(nameof(function));
			}
			return RunSTA(function, null);
		}

		/// <summary>
		/// Creates a new thread with <see cref="ApartmentState.STA"/> apartment state that runs the the specified work and returns a proxy for the task returned by function.
		/// </summary>
		/// <typeparam name="TResult">The type of the result returned by the proxy task.</typeparam>
		/// <param name="function">The work to execute asynchronously.</param>
		public static Task<TResult> RunSTA<TResult>(Func<Task<TResult>> function)
		{
			if(function == null) {
				throw new ArgumentNullException(nameof(function));
			}
			return RunSTA(null, function);
		}

		private static Task<TResult> RunSTA<TResult>(Func<TResult> function, Func<Task<TResult>> asyncFunction)
		{
			if(function == null && asyncFunction == null) {
				throw new ArgumentNullException("Both functions are null.");
			}
			if(function != null && asyncFunction != null) {
				throw new ArgumentException("Only one function should be provided.");
			}
			var tcs = new TaskCompletionSource<TResult>();
			ThreadStart threadStart;
			if(function != null) {
				threadStart = () =>
				{
					try {
						tcs.SetResult(function());
					} catch(Exception e) {
						tcs.SetException(e);
					}
				};
			} else {
				threadStart = async () =>
				{
					try {
						TResult result = await asyncFunction();
						tcs.SetResult(result);
					} catch(Exception e) {
						tcs.SetException(e);
					}
				};
			}
			var thread = new Thread(threadStart);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			return tcs.Task;
		}
	}
}
