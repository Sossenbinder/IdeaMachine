using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Account.Abstractions.Events.Interface;

namespace IdeaMachine.Modules.Account.Events
{
	public class AccountEvents : IAccountEvents
	{
		public IDistributedEvent<AccountCreated> AccountCreated { get; }

		public IDistributedEvent<AccountSignedIn> AccountSignedIn { get; }

		public IDistributedEvent<AccountLoggedOut> AccountSignedOut { get; }

		public IDistributedEvent<AccountVerified> AccountVerified { get; }

		public AccountEvents(
			IDistributedEvent<AccountCreated> accountCreated,
			IDistributedEvent<AccountSignedIn> accountSignedIn,
			IDistributedEvent<AccountLoggedOut> accountLoggedOut,
			IDistributedEvent<AccountVerified> accountVerified)
		{
			AccountCreated = accountCreated;
			AccountSignedIn = accountSignedIn;
			AccountSignedOut = accountLoggedOut;
			AccountVerified = accountVerified;
		}
	}
}