using IdeaMachine.Common.Core.Utils.Async;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Implementations;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache
{
	public class RedisCacheFactory
	{
		private readonly AsyncLazy<IConnectionMultiplexer> _lazyConnectionMultiplexer;

		public RedisCacheFactory(AsyncLazy<IConnectionMultiplexer> lazyConnectionMultiplexer)
		{
			_lazyConnectionMultiplexer = lazyConnectionMultiplexer;
		}

		public async Task<RedisCache<TKey, TValue>> Create<TKey, TValue>()
		{
			var connectionMultiplexer = await _lazyConnectionMultiplexer;
			return new RedisCache<TKey, TValue>(connectionMultiplexer);
		}
	}
}