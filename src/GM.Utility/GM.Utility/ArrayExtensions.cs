using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// Adds the provided item to the end of the array.
		/// </summary>
		/// <typeparam name="T">Type of the items in the array.</typeparam>
		/// <param name="array">The array to add the item to.</param>
		/// <param name="item">The item to add to the array.</param>
		public static T[] Add<T>(this T[] array, T item)
		{
			Array.Resize(ref array, array.Length + 1);
			array[array.Length - 1] = item;
			return array;
		}

		/// <summary>
		/// Performs the specified action for each element in the array. Supports multiple dimensions, the second parameter of the action are current indices for the dimensions.
		/// <para>
		/// To use the indices, you can use the <see cref="Array.GetValue(int[])"/> or <see cref="Array.SetValue(object, int[])"/>.
		/// </para>
		/// </summary>
		/// <param name="array">The array that contains the elements.</param>
		/// <param name="action">An action to invoke for each element. The second parameter is an int array that represents the indexes of the current element for each dimension.</param>
		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if(array.LongLength == 0) {
				return;
			}

			ArrayTraverse walker = new ArrayTraverse(array);

			do {
				action(array, walker.Position);
			}
			while(walker.Step());
		}

		private class ArrayTraverse
		{
			public int[] Position;
			private int[] maxLengths;

			public ArrayTraverse(Array array)
			{
				Position = new int[array.Rank];

				maxLengths = new int[array.Rank];
				for(int i = 0; i < array.Rank; i++)
					maxLengths[i] = array.GetLength(i) - 1;
			}

			public bool Step()
			{
				for(int i = 0; i < Position.Length; i++)
					if(Position[i] < maxLengths[i]) {
						Position[i]++;
						for(int j = 0; j < i; j++)
							Position[j] = 0;

						return true;
					}

				return false;
			}
		}
	}
}
