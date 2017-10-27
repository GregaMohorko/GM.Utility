using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for Dictionary.
	/// </summary>
	public static class DictionaryUtility
	{
		/// <summary>
		/// Creates two dictionaries: singles, that contains objects that are the only ones with a unique key, and lists, that contains lists of objects with the same key. Key is selected with the provided key selecting function.
		/// </summary>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TObject">Type of the objects.</typeparam>
		/// <param name="objects">List of objects which to sort into the two dictionaries.</param>
		/// <param name="singles">Dictionary with objects that have unique keys.</param>
		/// <param name="lists">Dictionary with lists of objects that have the same key.</param>
		/// <param name="keySelector">Function with which to select the key from an object.</param>
		public static void CreateKeyDictionaries<TKey, TObject>(List<TObject> objects, out Dictionary<TKey, TObject> singles, out Dictionary<TKey, List<TObject>> lists, Func<TObject, TKey> keySelector)
		{
			singles = new Dictionary<TKey, TObject>();
			lists = new Dictionary<TKey, List<TObject>>();

			IEnumerable<IGrouping<TKey, TObject>> groups = objects.GroupBy(keySelector);

			foreach(IGrouping<TKey, TObject> group in groups) {
				List<TObject> groupObjects = group.ToList();

				if(groupObjects.Count > 1) {
					lists.Add(group.Key, groupObjects);
				} else {
					singles.Add(group.Key, groupObjects[0]);
				}
			}
		}
	}
}
