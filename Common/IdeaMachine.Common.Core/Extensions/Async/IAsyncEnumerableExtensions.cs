using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions.Async
{
	public static class IAsyncEnumerableExtensions
	{
		public static async Task<IEnumerable<T>> ConsumeAsEnumerable<T>(this IAsyncEnumerable<T> asyncEnumerable)
		{
			var items = new List<T>();

			await foreach (var item in asyncEnumerable)
			{
				items.Add(item);
			}

			return items;
		}
	}
}