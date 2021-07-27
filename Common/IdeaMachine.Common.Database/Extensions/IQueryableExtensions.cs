using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

		public static async Task AddOrUpdate<T>(this DbSet<T> dbSet, T item, Expression<Func<T, bool>> match)
			where T : class
		{
			var itemExists = await dbSet.AsNoTracking().Where(match).AnyAsync();

			if (itemExists)
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