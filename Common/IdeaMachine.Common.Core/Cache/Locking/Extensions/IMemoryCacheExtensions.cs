using System;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Locking.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IMemoryCacheExtensions
	{
		public static T GetOrCreateOnce<T>(this IMemoryCache cache, object key, Func<ICacheEntry, T> factory)
		{
			Lazy<T>? creator = null;
			var result = cache.GetOrCreate(key, entry => (creator ??= new Lazy<T>(() => factory(entry))).Value);

			return result;
		}
	}
}