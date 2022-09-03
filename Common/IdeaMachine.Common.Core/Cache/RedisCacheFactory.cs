using IdeaMachine.Common.Core.Utils.Async;
using IdeaMachine.Common.Core.Cache.Implementations;
using IdeaMachine.Common.Core.Utils.Serialization;
using StackExchange.Redis;

namespace IdeaMachine.Common.Core.Cache
{
	public class RedisCacheFactory
	{
		private readonly AsyncLazy<IConnectionMultiplexer> _lazyConnectionMultiplexer;

		private readonly ISerializerDeserializer _serializerDeserializer;

		public RedisCacheFactory(
			AsyncLazy<IConnectionMultiplexer> lazyConnectionMultiplexer,
			ISerializerDeserializer serializerDeserializer)
		{
			_lazyConnectionMultiplexer = lazyConnectionMultiplexer;
			_serializerDeserializer = serializerDeserializer;
		}

		public RedisCache<TKey, TValue> Create<TKey, TValue>() where TKey : notnull
		{
			return new RedisCache<TKey, TValue>(_lazyConnectionMultiplexer, _serializerDeserializer);
		}
	}
}