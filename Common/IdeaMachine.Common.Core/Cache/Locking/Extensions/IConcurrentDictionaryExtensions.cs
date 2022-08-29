using System;
using System.Collections.Concurrent;

namespace IdeaMachine.Common.Core.Cache.Locking.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IConcurrentDictionaryExtensions
	{
		public static TValue GetOrAddOnce<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, Func<TValue> factory)
			where TKey : notnull
		{
			Lazy<TValue>? lazyVal = null;
			return dict.GetOrAdd(key, _ => (lazyVal ??= new Lazy<TValue>(factory)).Value);
		}
	}
}