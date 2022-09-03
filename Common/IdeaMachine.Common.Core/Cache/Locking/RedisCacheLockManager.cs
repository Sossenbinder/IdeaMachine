using IdeaMachine.Common.Core.Cache.Locking.Locks;
using IdeaMachine.Common.Core.Utils.Async;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class RedisCacheLockManager<TKey> : AbstractCacheLockManager<TKey>
		where TKey : notnull
	{
		public RedisCacheLockManager(AsyncLazy<IConnectionMultiplexer> redisConnectionMultiplexer)
			: base(key => new RedisLock(redisConnectionMultiplexer, key))
		{
		}
	}
}