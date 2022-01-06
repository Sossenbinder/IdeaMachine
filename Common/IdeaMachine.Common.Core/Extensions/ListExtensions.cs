using System;
using System.Collections.Generic;
using System.Linq;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class ListExtensions
	{
		public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> toRemove)
		{
			foreach (var itemToRemove in toRemove)
			{
				list.Remove(itemToRemove);
			}
		}

		public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> removePredicate)
		{
			// We need to .ToList() the .Where predicated list since otherwise the generator would iterate while we already remove
			list.RemoveRange(list.Where(removePredicate).ToList());
		}
	}
}