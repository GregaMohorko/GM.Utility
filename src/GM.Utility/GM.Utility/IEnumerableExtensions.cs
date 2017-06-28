using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Determines whether the provided collection is null or contains no elements.
		/// </summary>
		/// <typeparam name="T">The IEnumerable type.</typeparam>
		/// <param name="enumerable">The collection, which may be null or empty.</param>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			// Solution from: http://danielvaughan.org/post/IEnumerable-IsNullOrEmpty.aspx
			if(enumerable == null)
				return true;

			// if this is a collection, use the Count property
			// the count property is O(1) while IEnumerable.Count() is O(N)
			ICollection<T> collection = enumerable as ICollection<T>;
			if(collection != null)
				return collection.Count < 1;
			return enumerable.Any();
		}

		/// <summary>
		/// Returns distinct elements from a sequence using the provided value selector for equality comparison.
		/// </summary>
		/// <typeparam name="T1">The type of the elements of source.</typeparam>
		/// <typeparam name="T2">The type of the value on which to distinct.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="valueSelector">A transform function to apply to each element to select the value on which to distinct.</param>
		public static IEnumerable<T2> Distinct<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> valueSelector)
		{
			return source.Select(valueSelector).Distinct();
		}

		/// <summary>
		/// Determines whether two sequences are equal by comparing the elements by using the default equality comparer for their type. Note that this method does not compare the order of elements and returns true even if the order is not equal.
		/// </summary>
		/// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
		/// <param name="first">The first sequence.</param>
		/// <param name="second">An IEnumerable to compare to the first sequence.</param>
		public static bool SequenceEqualUnordered<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			// Solution from: http://stackoverflow.com/questions/22173762/check-if-two-lists-are-equal
			// linear time!

			// hash-based dictionary of counts
			Dictionary<T, int> counts = first.GroupBy(element => element).ToDictionary(group => group.Key, group => group.Count());

			// -1 for each element of the second sequence
			bool ok = true;
			foreach(T element in second) {
				if(counts.ContainsKey(element))
					counts[element]--;
				else {
					ok = false;
					break;
				}
			}

			// check if the resultant counts are all zeros
			return ok && counts.Values.All(count => count == 0);
		}
	}
}
