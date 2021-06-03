using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Disposable;
using IdeaMachine.Common.Core.Utils.Collections;
using IdeaMachine.Common.Eventing.Abstractions.Events;

namespace IdeaMachine.Common.Eventing.Events
{
	public abstract class EventBase : IEvent
	{
		protected readonly ConcurrentHashSet<Func<Task>> Handlers = new();

		public virtual DisposableAction Register(Func<Task> handler)
		{
			Handlers.Add(handler);

			return new DisposableAction(() => Unregister(handler));
		}

		public void Unregister(Func<Task> handler)
		{
			Handlers.Remove(handler);
		}

		internal List<Func<Task>> GetAllRegisteredEvents()
		{
			return Handlers.ToList();
		}
	}

	public abstract class EventBase<TEventArgs> : IEvent<TEventArgs>
	{
		protected readonly ConcurrentHashSet<Func<TEventArgs, Task>> Handlers = new();

		public virtual DisposableAction Register(Func<TEventArgs, Task> handler)
		{
			Handlers.Add(handler);

			return new DisposableAction(() => UnRegister(handler));
		}

		public void UnRegister(Func<TEventArgs, Task> handler)
		{
			Handlers.Remove(handler);
		}

		internal List<Func<TEventArgs, Task>> GetAllRegisteredEvents()
		{
			return Handlers.ToList();
		}
	}
}