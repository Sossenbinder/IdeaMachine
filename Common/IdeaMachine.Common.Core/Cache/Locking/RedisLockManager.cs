using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;
using IdeaMachine.Common.Core.Cache.Locking.Locks;
using Microsoft.Extensions.Caching.Memory;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class RedisLockManager<TKey> : AbstractCacheLockManager<TKey>
	{
		protected RedisLockManager()
			: base(disposeFunc => new RedisLock(null, default, default))
		{
		}
	}
}