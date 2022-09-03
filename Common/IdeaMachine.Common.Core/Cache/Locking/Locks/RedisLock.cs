using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Async;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache.Locking.Locks
{
	public class RedisLock : AbstractCacheLock
	{
		private readonly AsyncLazy<IConnectionMultiplexer> _redisConnectionMultiplexer;

		private readonly string _key;

		private readonly RedisValue _value;

		private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

		public RedisLock(
			AsyncLazy<IConnectionMultiplexer> redisConnectionMultiplexer,
			string key)
		{
			_redisConnectionMultiplexer = redisConnectionMultiplexer;
			_key = key;
			_value = Guid.NewGuid().ToString();
		}

		public override async Task Lock(TimeSpan? expirationTimeSpan = default)
		{
			await (await GetDb()).LockTakeAsync(_key, _value, expirationTimeSpan ?? _defaultTimeout);
		}

		public override async ValueTask Release()
		{
			await (await GetDb()).LockReleaseAsync(_key, _value);
		}

		private async ValueTask<IDatabase> GetDb()
		{
			return (await _redisConnectionMultiplexer).GetDatabase();
		}
	}
}