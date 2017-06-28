using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class Util
	{
		public static void Swap<T>(ref T A,ref T B)
		{
			T swap = A;
			A = B;
			B = swap;
		}

		public static void Swap(double[] array, int index1, int index2)
		{
			Swap(ref array[index1], ref array[index2]);
		}

		public static void Swap(double[,] array, int row1, int row2)
		{
			for(int i = array.GetLength(1) - 1; i >= 0; --i)
				Swap(ref array[row1, i], ref array[row2, i]);
		}
	}
}
