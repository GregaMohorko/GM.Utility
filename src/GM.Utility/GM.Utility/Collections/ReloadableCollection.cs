/*
MIT License

Copyright (c) 2018 Grega Mohorko

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
Created: 2018-1-6
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GM.Utility.Collections
{
	/// <summary>
	/// An ObservableCollection that reloads with an interval.
	/// <para>
	/// Important: Needs to be stopped and/or disposed.
	/// </para>
	/// </summary>
	public class ReloadableCollection<T> : ObservableCollection<T>, IDisposable
	{
		/// <summary>
		/// Default interval (in milliseconds) with which collections will reload.
		/// </summary>
		public const int DEFAULT_INTERVAL = 1000;

		/// <summary>
		/// The interval (in milliseconds) with which this collection is reloading.
		/// </summary>
		public int Interval
		{
			get => _interval;
			set => _interval = value;
		}
		private volatile int _interval;

		/// <summary>
		/// If set, the <see cref="Collection{T}.Add(T)"/> and <see cref="ObservableCollection{T}.RemoveItem(int)"/> operations will be invoked on this dispatcher.
		/// </summary>
		public readonly Dispatcher Dispatcher;

		private readonly Func<List<T>> loadItems;
		private Task task;
		private volatile CancellationTokenSource cts;
		private volatile int currentStartID;
		private volatile int currentStopID;

		/// <summary>
		/// Initializes a new instance of <see cref="ReloadableCollection{T}"/>.
		/// </summary>
		/// <param name="interval">The interval (in milliseconds) with which this collection will be reloading.</param>
		/// <param name="loadItems">A method that (re)loads the elements of this collection.</param>
		/// <param name="dispatcher">If set, the <see cref="Collection{T}.Add(T)"/> and <see cref="ObservableCollection{T}.RemoveItem(int)"/> operations will be invoked on this dispatcher.</param>
		/// <param name="startImmediately">If true, reloading will start immediately when this instance is initialized.</param>
		public ReloadableCollection(int interval, Func<List<T>> loadItems, Dispatcher dispatcher=null, bool startImmediately = false) : base()
		{
			currentStartID = 0;
			currentStopID = 0;
			Interval = interval;
			this.loadItems = loadItems;
			Dispatcher = dispatcher;

			if(startImmediately) {
				Start();
			}
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ReloadableCollection{T}"/> with the <see cref="DEFAULT_INTERVAL"/> interval.
		/// </summary>
		/// <param name="loadItems">A method that (re)loads the elements of this collection.</param>
		/// <param name="dispatcher">If set, the <see cref="Collection{T}.Add(T)"/> and <see cref="ObservableCollection{T}.RemoveItem(int)"/> operations will be invoked on this dispatcher.</param>
		/// <param name="startImmediately">If true, reloading will start immediately when this instance is initialized.</param>
		public ReloadableCollection(Func<List<T>> loadItems,Dispatcher dispatcher=null, bool startImmediately = false) : this(DEFAULT_INTERVAL, loadItems,dispatcher, startImmediately) { }

		/// <summary>
		/// Starts reloading this collection.
		/// </summary>
		public void Start()
		{
			if(currentStartID > currentStopID) {
				// start is more, running already? do nothing
			} else if(currentStartID == currentStopID) {
				// are the same, start fresh

				++currentStartID;

				// check if task is null OR if it is already completed (if its not, it will restart the thread again when the current ends)
				if(task == null || task.IsCompleted) {
					StartInternal();
				}
			} else {
				throw new NotImplementedException("WTF"); // it should never come to this
			}
		}

		/// <summary>
		/// Actually fires up the inner thread and starts running.
		/// </summary>
		private void StartInternal()
		{
			cts = new CancellationTokenSource();

			task = new Task(Run, cts.Token, TaskCreationOptions.LongRunning);

			task.Start();
		}

		/// <summary>
		/// Stops the background reloading. If the wait parameter is true, it will wait for the background task to stop executing.
		/// </summary>
		/// <param name="wait">True, if it should wait for the reloading background task to stop executing.</param>
		public void Stop(bool wait)
		{
			if(currentStartID > currentStopID) {
				// running, stop it

				++currentStopID;
				cts?.Cancel();
				if(wait && !task.IsCompleted) {
					task.Wait();
				}
			} else if(currentStartID == currentStopID) {
				// if Stop(false) was called, and then Stop(true) BEFORE it has actually stopped, it can come to here
				if(wait && task != null && !task.IsCompleted) {
					task.Wait();
				}
			} else {
				throw new NotImplementedException("WTF"); // it should never come to this
			}
		}

		private void Run()
		{
			while(!cts.Token.IsCancellationRequested) {
				Reload();
				Thread.Sleep(Interval);
			}

			if(currentStartID > currentStopID) {
				// new start is pending, fire it up
				StartInternal();
			}
		}

		private void Reload()
		{
			if(loadItems == null) {
				Stop(false);
				return;
			}

			List<T> reloadedItems = loadItems.Invoke();
			if(reloadedItems == null || cts.Token.IsCancellationRequested) {
				return;
			}

			IList<T> originalList = Items;

			if(originalList.Count == 0) {
				foreach(T item in reloadedItems) {
					if(Dispatcher != null) {
						Dispatcher.Invoke(() => Add(item));
					} else {
						Add(item);
					}
				}
				return;
			}

			List<T> itemsToInsert = new List<T>();
			bool[] checkedIndexes = new bool[originalList.Count];

			for(int i = 0; i < reloadedItems.Count; i++) {
				T reloadedItem = reloadedItems[i];

				int currentItemIndex = originalList.IndexOf(reloadedItem);

				if(currentItemIndex == -1) {
					itemsToInsert.Add(reloadedItem);
				} else {
					checkedIndexes[currentItemIndex] = true;
				}
			}

			// check if any items have not been checked (were probably deleted somewhere outside of this application)
			for(int i = checkedIndexes.Length - 1; i >= 0; i--) {
				if(!checkedIndexes[i]) {
					T itemToRemove = originalList[i];

					int index = IndexOf(itemToRemove);

					if(Dispatcher != null) {
						Dispatcher.Invoke(() => RemoveItem(index));
					} else {
						RemoveItem(index);
					}
				}
			}

			foreach(T itemToInsert in itemsToInsert) {
				if(Dispatcher != null) {
					Dispatcher.Invoke(() => Add(itemToInsert));
				} else {
					Add(itemToInsert);
				}
			}
		}

		/// <summary>
		/// Disposes this instance of <see cref="ReloadableCollection{T}"/>. If the reloading task is currently running, it waits for it to stop.
		/// </summary>
		public void Dispose()
		{
			Stop(true);

			task?.Dispose();
			cts?.Dispose();
		}
	}
}
