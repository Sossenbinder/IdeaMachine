using System;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Locking.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IMemoryCacheExtensions
	{
		public static T GetOrCreateOnce<T>(this IMemoryCache cache, object key, Func<T> factory)
		{
			var creator = new Lazy<T>(factory);
			return cache.GetOrCreate(key, _ => creator.Value);
		}
	}
}
