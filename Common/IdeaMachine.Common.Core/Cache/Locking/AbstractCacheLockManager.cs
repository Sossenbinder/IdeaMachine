using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Extensions;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Locking
{
    public class AbstractCacheLockManager<TKey> : ICacheLockManager<TKey> 
	    where TKey : notnull
    {
	    private readonly Func<ICacheLock> _cacheLockFactory;

	    private readonly IMemoryCache _cacheLocks;

	    protected AbstractCacheLockManager(Func<ICacheLock> cacheLockFactory)
	    {
		    _cacheLockFactory = cacheLockFactory;
		    _cacheLocks = new MemoryCache(new MemoryCacheOptions());
	    }

	    public async Task<ICacheLock> GetLockLocked(TKey key)
	    {
		    var @lock = _cacheLocks.GetOrCreateOnce(key, _cacheLockFactory);
		    await @lock.Lock();
		    return @lock;
	    }

	    public void ReleaseLock(TKey key)
	    {
		    _cacheLocks.Remove(key);
	    }
    }
}
