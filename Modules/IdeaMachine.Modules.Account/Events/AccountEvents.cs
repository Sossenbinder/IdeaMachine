using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Events;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Account.DataTypes.Events;
using IdeaMachine.Modules.Account.Events.Interface;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.Modules.Account.Events
{
	public class AccountEvents : IAccountEvents
	{
		public IDistributedEvent<AccountCreated> AccountCreated { get; }

		public AccountEvents(
			IMassTransitEventingService massTransitEventingService,
			ILogger<AccountEvents> logger)
		{
			AccountCreated = new MtEvent<AccountCreated>(massTransitEventingService, logger);
		}
	}
}