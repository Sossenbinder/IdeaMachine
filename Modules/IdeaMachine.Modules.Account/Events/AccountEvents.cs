using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;

namespace IdeaMachine.Modules.Account.Events
{
	public class AccountEvents : IAccountEvents
	{
		public IDistributedEvent<AccountLoggedOut> AccountSignedOut { get; }

		public AccountEvents(IDistributedEvent<AccountLoggedOut> accountLoggedOut)
		{
			AccountSignedOut = accountLoggedOut;
		}
	}
}