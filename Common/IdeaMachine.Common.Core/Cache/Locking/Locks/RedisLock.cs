using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache.Locking.Locks
{
	public class RedisLock : AbstractCacheLock
	{
		private readonly IDatabase _redisDatabase;

		private readonly RedisKey _key;

		private readonly RedisValue _value;

		private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

		public RedisLock(
			IDatabase redisDatabase,
			RedisKey key,
			RedisValue value)
		{
			_redisDatabase = redisDatabase;
			_key = key;
			_value = value;
		}

		public override Task Lock(TimeSpan? expirationTimeSpan = default)
		{
			return _redisDatabase.LockTakeAsync(_key, _value, expirationTimeSpan ?? _defaultTimeout);
		}

		public override async ValueTask Release()
		{
			await _redisDatabase.LockReleaseAsync(_key, _value);
		}
	}
}