using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IdeaMachine.Common.Core.Utils.Collections
{
	public class ConcurrentHashSet<T> : IEnumerable<T>
	{
		private readonly HashSet<T> _hashSet;

		private readonly object _lock = new();

		public ConcurrentHashSet()
		{
			_hashSet = new HashSet<T>();
		}

		public ConcurrentHashSet(IEnumerable<T> collection)
		{
			_hashSet = collection.ToHashSet();
		}

		public int Count
		{
			get
			{
				lock (_lock)
				{
					return _hashSet.Count;
				}
			}
		}

		public bool Add(T item)
		{
			lock (_lock)
			{
				return _hashSet.Add(item);
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_hashSet.Clear();
			}
		}

		public bool Contains(T item)
		{
			lock (_lock)
			{
				return _hashSet.Contains(item);
			}
		}

		public bool Remove(T item)
		{
			lock (_lock)
			{
				return _hashSet.Remove(item);
			}
		}

		public int RemoveWhere(Predicate<T> predicate)
		{
			lock (_lock)
			{
				return _hashSet.RemoveWhere(predicate);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			lock (_lock)
			{
				return _hashSet.GetEnumerator();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}