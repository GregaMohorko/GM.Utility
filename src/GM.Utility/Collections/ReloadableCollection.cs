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

namespace GM.Utility.Collections
{
	/// <summary>
	/// An <see cref="ObservableCollection{T}"/> that reloads with an interval.
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
		public int Interval { get; set; }

		private readonly Func<List<T>> loadItemsSync;
		private readonly Func<Task<List<T>>> loadItemsAsync;
		private CancellationTokenSource cts;
		private int currentStartID;
		private int currentStopID;

		/// <summary>
		/// Initializes a new instance of <see cref="ReloadableCollection{T}"/> with synchronous reloading and the <see cref="DEFAULT_INTERVAL"/> interval.
		/// </summary>
		/// <param name="loadItems">A method that (re)loads the elements of this collection.</param>
		/// <param name="startImmediately">If true, reloading will start immediately when this instance is initialized.</param>
		public ReloadableCollection(Func<List<T>> loadItems, bool startImmediately = false) : this(DEFAULT_INTERVAL, loadItems, startImmediately) { }

		/// <summary>
		/// Initializes a new instance of <see cref="ReloadableCollection{T}"/> with synchronous reloading.
		/// </summary>
		/// <param name="interval">The interval (in milliseconds) with which this collection will be reloading.</param>
		/// <param name="loadItems">A method that (re)loads the elements of this collection.</param>
		/// <param name="startImmediately">If true, reloading will start immediately when this instance is initialized.</param>
		public ReloadableCollection(int interval, Func<List<T>> loadItems, bool startImmediately = false) : this(interval,loadItems, null,startImmediately) { }

		/// <summary>
		/// Initializes a new instance of <see cref="ReloadableCollection{T}"/> with asynchronous reloading and the <see cref="DEFAULT_INTERVAL"/> interval.
		/// </summary>
		/// <param name="loadItems">A method that (re)loads the elements of this collection.</param>
		/// <param name="startImmediately">If true, reloading will start immediately when this instance is initialized.</param>
		public ReloadableCollection(Func<Task<List<T>>> loadItems,bool startImmediately = false) : this(DEFAULT_INTERVAL, loadItems, startImmediately) { }

		/// <summary>
		/// Initializes a new instance of <see cref="ReloadableCollection{T}"/> with asynchronous reloading.
		/// </summary>
		/// <param name="interval">The interval (in milliseconds) with which this collection will be reloading.</param>
		/// <param name="loadItems">A method that (re)loads the elements of this collection.</param>
		/// <param name="startImmediately">If true, reloading will start immediately when this instance is initialized.</param>
		public ReloadableCollection(int interval, Func<Task<List<T>>> loadItems, bool startImmediately = false) : this(interval, null, loadItems, startImmediately) { }

		private ReloadableCollection(int interval, Func<List<T>> loadItemsSync, Func<Task<List<T>>> loadItemsAsync, bool startImmediately)
		{
			currentStartID = 0;
			currentStopID = 0;
			Interval = interval;
			this.loadItemsSync = loadItemsSync;
			this.loadItemsAsync = loadItemsAsync;
			if(startImmediately) {
				Start();
			}
		}

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

				// if cts != null it means that it is currently still running and that it will end the next iteration
				// and it will see that a new start is pending and will start it
				if(cts == null) {
					StartInternal();
				}
			} else {
				throw new NotImplementedException("WTF"); // it should never come to this
			}
		}

		/// <summary>
		/// Stops the reloading.
		/// </summary>
		public void Stop()
		{
			if(currentStartID > currentStopID) {
				// running, stop it

				++currentStopID;
				cts?.Cancel();
			} else if(currentStartID == currentStopID) {
				// already stopped, do nothing
			} else {
				throw new NotImplementedException("WTF"); // it should never come to this
			}
		}

		private async void StartInternal()
		{
			cts = new CancellationTokenSource();

			while(!cts.Token.IsCancellationRequested) {
				await Reload();
				await Task.Delay(Interval);
			}

			cts.Dispose();
			cts = null;

			if(currentStartID > currentStopID) {
				// new start is pending, fire it up
				StartInternal();
			}
		}

		private async Task Reload()
		{
			List<T> reloadedItems;
			if(loadItemsAsync != null) {
				reloadedItems = await loadItemsAsync();
			} else {
				reloadedItems = loadItemsSync();
			}
			if(reloadedItems == null || cts.Token.IsCancellationRequested) {
				return;
			}

			IList<T> originalList = Items;

			if(originalList.Count == 0) {
				foreach(T item in reloadedItems) {
					Add(item);
				}
				return;
			}

			// checks which reloaded items should be inserted
			List<T> itemsToInsert = new List<T>();
			bool[] checkedIndexes = new bool[originalList.Count];
			for(int i = 0; i < reloadedItems.Count; ++i) {
				T reloadedItem = reloadedItems[i];

				int currentItemIndex = originalList.IndexOf(reloadedItem);

				if(currentItemIndex == -1) {
					itemsToInsert.Add(reloadedItem);
				} else {
					checkedIndexes[currentItemIndex] = true;
				}
			}

			// check if any items have not been checked (were probably deleted somewhere outside of this application)
			for(int i = checkedIndexes.Length - 1; i >= 0; --i) {
				if(!checkedIndexes[i]) {
					T itemToRemove = originalList[i];

					int index = IndexOf(itemToRemove);

					RemoveItem(index);
				}
			}

			foreach(T itemToInsert in itemsToInsert) {
				Add(itemToInsert);
			}
		}

		/// <summary>
		/// Stops and disposes this instance of <see cref="ReloadableCollection{T}"/>.
		/// </summary>
		public void Dispose()
		{
			Stop();
		}
	}
}
