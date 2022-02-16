using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Disposable;

namespace IdeaMachine.Common.Eventing.Abstractions.Events
{
	public interface IEvent
	{
		DisposableAction Register(Func<Task> handler);

		void UnRegister(Func<Task> handler);
	}

	/// <summary>
	/// Exposes methods for basic event registrations
	/// </summary>
	public interface IEvent<out TEventArgs>
	{
		DisposableAction Register(Func<TEventArgs, Task> handler);

		void UnRegister(Func<TEventArgs, Task> handler);
	}
}