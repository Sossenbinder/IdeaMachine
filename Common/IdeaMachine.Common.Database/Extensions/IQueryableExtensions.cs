using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IdeaMachine.Common.Database.Extensions
{
	public static class IQueryableExtensions
	{
		public static async Task<HashSet<T>> ToHashSetAsync<T>(this IQueryable<T> queryable)
		{
			var entrySet = new HashSet<T>();
			await foreach (var entry in queryable.AsAsyncEnumerable())
			{
				entrySet.Add(entry);
			}
			return entrySet;
		}

		public static async Task AddOrUpdate<T, TKey>(this DbSet<T> dbSet, T item, Func<T, TKey> keyFunc)
			where T : class
		{
			var knownKeys = await dbSet.AsNoTracking().Select(x => keyFunc(x)).ToListAsync();

			var itemKey = keyFunc(item);

			if (knownKeys.Any(key => itemKey.Equals(key)))
			{
				dbSet.Update(item);
			}
			else
			{
				dbSet.Add(item);
			}
		}

		public static async Task AddOrUpdateRange<T, TKey>(this DbSet<T> dbSet, IEnumerable<T> items, Func<T, TKey> keyFunc)
			where T : class
		{
			var knownKeys = await dbSet.AsNoTracking().Select(x => keyFunc(x)).ToListAsync();

			foreach (var item in items)
			{
				var itemKey = keyFunc(item);

				if (knownKeys.Any(key => itemKey.Equals(key)))
				{
					dbSet.Update(item);
				}
				else
				{
					dbSet.Add(item);
				}
			}
		}
	}
}