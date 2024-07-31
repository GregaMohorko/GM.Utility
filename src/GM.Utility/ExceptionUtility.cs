using System;
using System.Collections.Generic;
using System.Text;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for <see cref="Exception"/>.
	/// </summary>
	public static class ExceptionUtility
	{
		/// <summary>
		/// Returns the inner-most exception of this exception.
		/// <para>Loops the <see cref="Exception.InnerException"/> to the end.</para>
		/// </summary>
		/// <param name="ex">The exception from which to get the inner-most exception.</param>
		public static Exception GetInnermostException(this Exception ex)
		{
			if(ex == null) {
				throw new ArgumentNullException(nameof(ex));
			}
			Exception innermost = ex;
			while(innermost.InnerException != null) {
				innermost = innermost.InnerException;
			}
			return innermost;
		}
	}
}
