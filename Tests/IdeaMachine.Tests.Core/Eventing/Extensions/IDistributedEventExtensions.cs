using IdeaMachine.Common.Eventing.Abstractions.Events;
using MassTransit.Testing;

namespace IdeaMachine.Tests.Core.Eventing.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class IDistributedEventExtensions
	{
		public static async Task<bool> RaiseForTest<T>(this IDistributedEvent<T> @event, BusTestHarness busTestHarness, T eventArgs) 
			where T : class
		{
			await @event.Raise(eventArgs);
			return await busTestHarness.Consumed.Any<T>();
		}
	}
}
