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

Project: GM.Utility.Test
Created: 2018-12-13
Author: GregaMohorko
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GM.Utility.Collections
{
	/// <summary>
	/// A generic collection that supports data binding and sorting.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	public class SortableBindingList<T> : BindingList<T>
	{
		/// <summary>
		/// Gets a value indicating whether the list supports sorting.
		/// </summary>
		protected override bool SupportsSortingCore => true;
		/// <summary>
		/// Gets a value indicating whether the list is sorted.
		/// </summary>
		protected override bool IsSortedCore => isSorted;
		/// <summary>
		/// Gets the direction the list is sorted.
		/// </summary>
		protected override ListSortDirection SortDirectionCore => sortDirection;
		/// <summary>
		/// Gets the property descriptor that is used for sorting the list.
		/// </summary>
		protected override PropertyDescriptor SortPropertyCore => sortProperty;

		private bool isSorted;
		private ListSortDirection sortDirection;
		private PropertyDescriptor sortProperty;

		/// <summary>
		/// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class using default values.
		/// </summary>
		public SortableBindingList() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="SortableBindingList{T}"/> class with the specified list.
		/// </summary>
		/// <param name="enumerable"></param>
		public SortableBindingList(IEnumerable<T> enumerable) : base(enumerable.ToList()) { }

		/// <summary>
		/// Sorts the items.
		/// </summary>
		/// <param name="prop">A <see cref="PropertyDescriptor"/> that specifies the property to sort on.</param>
		/// <param name="direction">One of the <see cref="ListSortDirection"/> values.</param>
		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			isSorted = true;
			sortDirection = direction;
			sortProperty = prop;

			PropertyInfo propertyInfo = typeof(T).GetProperty(prop.Name);
			object keySelector(T i) => propertyInfo.GetValue(i);

			if(Items.Count > 10000) {
				ResetItems(sortDirection == ListSortDirection.Ascending
					? Items.AsParallel().OrderBy(keySelector)
					: Items.AsParallel().OrderByDescending(keySelector));
			} else {
				ResetItems(sortDirection == ListSortDirection.Ascending
					? Items.OrderBy(keySelector)
					: Items.OrderByDescending(keySelector));
			}
		}

		/// <summary>
		/// Removes any sort applied with <see cref="ApplySortCore(PropertyDescriptor, ListSortDirection)"/>.
		/// </summary>
		protected override void RemoveSortCore()
		{
			isSorted = false;
			sortDirection = base.SortDirectionCore;
			sortProperty = base.SortPropertyCore;

			ResetBindings();
		}

		/// <summary>
		/// Searches for the index of the item that has the specified property descriptor with the specified value.
		/// </summary>
		/// <param name="prop">The <see cref="PropertyDescriptor"/> to search for.</param>
		/// <param name="key">The value of property to match.</param>
		protected override int FindCore(PropertyDescriptor prop, object key)
		{
			int count = Count;
			for(int i = 0; i < count; ++i) {
				T element = this[i];
				if(prop.GetValue(element).Equals(key)) {
					return i;
				}
			}

			return -1;
		}

		private void ResetItems(IEnumerable<T> items)
		{
			RaiseListChangedEvents = false;
			var tmpList = items.ToList();
			ClearItems();

			foreach(var item in tmpList) {
				Add(item);
			}

			RaiseListChangedEvents = true;
			ResetBindings();
		}
	}
}
