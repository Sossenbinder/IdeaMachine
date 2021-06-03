using System;

namespace IdeaMachine.Common.Core.Disposable
{
	/// <summary>
	/// Small wrapper allowing a simple Action to be treated as an IDisposable
	/// </summary>
	public class DisposableAction : IDisposable
	{
		private readonly Action _onDispose;

		public DisposableAction(Action onDispose)
		{
			_onDispose = onDispose;
		}

		public void Dispose() => _onDispose();
	}
}