using System;
using System.Collections;
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

		public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? enumerable)
		{
			return enumerable?.Any() ?? false;
		}

		public static IEnumerable<T> Apply<T>(this IEnumerable<T> enumerable, Action<T> transformerAction)
			where T : class
		{
			return new ForEachIterator<T>(enumerable, transformerAction);
		}
	}

	public class ForEachIterator<T> : IEnumerable<T>, IEnumerator<T>
	{
		public T? Current { get; private set; }

		object IEnumerator.Current => Current;

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

		public void Reset() => throw new NotImplementedException();

		public void Dispose()
		{
			Current = default;
			_enumerator = null;

			GC.SuppressFinalize(this);
		}
	}
}