using System.Collections.Generic;
using System.Linq;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
		{
			return !enumerable?.Any() ?? true;
		}
	}
}