using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Utils.Async
{
	public class AsyncLazy<T> : Lazy<Task<T>>
	{
		public AsyncLazy(Func<Task<T>> lazyInitializer, LazyThreadSafetyMode lazyThreadSafetyMode = LazyThreadSafetyMode.ExecutionAndPublication)
			: base(lazyInitializer, lazyThreadSafetyMode)
		{
		}

		public TaskAwaiter<T> GetAwaiter()
		{
			return Value.GetAwaiter();
		}
	}
}