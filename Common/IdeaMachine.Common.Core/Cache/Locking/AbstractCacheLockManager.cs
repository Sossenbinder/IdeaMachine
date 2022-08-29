using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class AbstractCacheLockManager<TKey> : ICacheLockManager<TKey>
		where TKey : notnull
	{
		private readonly Func<Action, ICacheLock> _cacheLockFactory;

		private readonly ConcurrentDictionary<TKey, ICacheLock> _cacheLocks;

		private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(10);

		protected AbstractCacheLockManager(Func<Action, ICacheLock> cacheLockFactory)
		{
			_cacheLockFactory = cacheLockFactory;
			_cacheLocks = new();
		}

		public async Task<ICacheLock> GetLockLocked(TKey key, TimeSpan? expirationTimeSpan = default)
		{
			var cts = new CancellationTokenSource(expirationTimeSpan ?? _defaultTimeout);

			var lockReceived = false;
			while (!cts.IsCancellationRequested && !lockReceived)
			{
				// Get the potential already existing lock and await it
				// This will ensure that we will not simply skip an existing lock
				if (_cacheLocks.TryGetValue(key, out var @lock))
				{
					await @lock.Lock(expirationTimeSpan);
				}
				else
				{
					// In case no lock was found, try to add this one as the "new" lock. Return it if successful, otherwise
					// rerun the loop and handle it just like a lock was already found.
					var newLock = _cacheLockFactory(() => ReleaseLock(key));
					if (_cacheLocks.TryAdd(key, newLock))
					{
						return newLock;
					}

					continue;
				}

				// The old lock was patiently awaited. Now we try to update it with ours
				var updatedLock = _cacheLockFactory(() => ReleaseLock(key));
				lockReceived = _cacheLocks.TryUpdate(key, updatedLock, @lock);

				if (lockReceived)
				{
					// We got lucky - Return this lock
					return updatedLock;
				}

				// If we did not get lucky - We have to rerun the while loop sadly.
			}

			throw new TaskCanceledException();
		}

		public void ReleaseLock(TKey key)
		{
			_cacheLocks.TryRemove(key, out _);
		}
	}
}