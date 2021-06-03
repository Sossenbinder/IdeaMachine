using System;
using System.Threading.Tasks;

namespace IdeaMachine.Common.Core.Extensions
{
	/// <summary>
	/// Extensions capable of making an Action fit nicely into a <see cref="Func&lt;T, Task&gt;;"/>
	/// </summary>
	public static class ActionExtensions
	{
		public static Func<Task>? MakeTaskCompatible(this Action? action)
		{
			if (action == null)
			{
				return null;
			}

			return () =>
			{
				action();
				return Task.CompletedTask;
			};
		}

		public static Func<T, Task>? MakeTaskCompatible<T>(this Action<T>? action)
		{
			if (action == null)
			{
				return null;
			}

			return x =>
			{
				action(x);
				return Task.CompletedTask;
			};
		}
	}
}