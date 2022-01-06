using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IdeaMachine.Common.Core.Extensions
{
	public static class IEnumerableExtensions
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable)
		{
			return !enumerable?.Any() ?? true;
		}

		public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? enumerable)
		{
			return enumerable?.Any() ?? false;
		}

		public static IEnumerable<T> Apply<T>(this IEnumerable<T> enumerable, Action<T> transformerAction)
			where T : class
		{
			return new ForEachIterator<T>(enumerable, transformerAction);
		}

		public static IEnumerable<(T Value, int Index)> AsIndexedIterable<T>(this IEnumerable<T> enumerable)
		{
			var list = enumerable.ToList();

			for (var i = 0; i < list.Count; ++i)
			{
				yield return (list[i], i);
			}
		}
	}

	public class ForEachIterator<T> : IEnumerable<T>, IEnumerator<T>
	{
		public T Current { get; private set; } = default!;

		object IEnumerator.Current => Current!;

		private readonly IEnumerable<T> _enumerable;

		private readonly Action<T> _action;

		private IEnumerator<T>? _enumerator;

		public ForEachIterator(
			IEnumerable<T> enumerable,
			Action<T> action)
		{
			_enumerable = enumerable;
			_action = action;
		}

		public IEnumerator<T> GetEnumerator() => this;

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool MoveNext()
		{
			_enumerator ??= _enumerable.GetEnumerator();

			if (!_enumerator.MoveNext())
			{
				return false;
			}

			var current = _enumerator.Current;

			_action(current);

			return true;
		}

		public void Reset() => _enumerator = _enumerable.GetEnumerator();

		public void Dispose()
		{
			Current = default;
			_enumerator = null;

			GC.SuppressFinalize(this);
		}
	}
}