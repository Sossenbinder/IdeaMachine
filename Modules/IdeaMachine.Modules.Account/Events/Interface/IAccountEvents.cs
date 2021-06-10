using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Account.DataTypes.Events;

namespace IdeaMachine.Modules.Account.Events.Interface
{
	public interface IAccountEvents
	{
		IDistributedEvent<AccountCreated> AccountCreated { get; }
	}
}