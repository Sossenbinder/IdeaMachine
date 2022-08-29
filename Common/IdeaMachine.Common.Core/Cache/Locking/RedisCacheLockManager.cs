using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Cache.Locking.Interface;

namespace IdeaMachine.Common.Core.Cache.Locking
{
	public class RedisCacheLockManager<TKey> : AbstractCacheLockManager<TKey>
	{
		protected RedisCacheLockManager()
			: base(CreateLock)
		{
		}

		private static ICacheLock CreateLock(Action disposeFunc)
		{
			return null;
		}
	}
}