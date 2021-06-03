using System;

namespace IdeaMachine.Common.Core.Utils
{
	public class ResetLazy<T>
	{
		private readonly Func<T> _factory;

		private Lazy<T> _lazy = null!;

		public T Value => _lazy.Value;

		public bool IsValueCreated => _lazy.IsValueCreated;

		public ResetLazy(Func<T> factory)
		{
			_factory = factory;
			CreateLazy();
		}

		public void Reset()
		{
			CreateLazy();
		}

		private void CreateLazy() => _lazy = new Lazy<T>(_factory);
	}
}