using System;
using System.Collections.Generic;

namespace IdeaMachine.Common.Core.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IDictionaryExtensions
	{
		public static void AddOrUpdate<TKey, TValue>(
			this IDictionary<TKey, TValue> dictionary,
			TKey key,
			TValue valueForAdd,
			Action<TValue> updateValueTransformer)
			where TKey : notnull
		{
			var addSuccess = dictionary.TryAdd(key, valueForAdd);

			if (addSuccess)
			{
				return;
			}

			var value = dictionary[key];

			updateValueTransformer(value);
		}

		public static bool TryGetValueTyped<TDictKey, TDictVal, TActualVal>(
			this IDictionary<TDictKey, TDictVal> dictionary,
			TDictKey key,
			out TActualVal? outVal)
			where TActualVal : TDictVal
		{
			if (!dictionary.TryGetValue(key, out var originalValue))
			{
				outVal = default;
				return false;
			}

			outVal = (TActualVal?)originalValue;
			return true;
		}
	}
}