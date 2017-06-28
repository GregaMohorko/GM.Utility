using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility
{
	public static class TypeExtensions
	{
		public static bool IsPrimitive(this Type type)
		{
			if(type == typeof(string))
				return true;

			return type.IsValueType && type.IsPrimitive;
		}
	}
}
