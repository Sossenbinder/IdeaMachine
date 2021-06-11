﻿using IdeaMachine.Common.Eventing.Abstractions.Events;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;

namespace IdeaMachine.Modules.Account.Abstractions.Events.Interface
{
	public interface IAccountEvents
	{
		IDistributedEvent<AccountCreated> AccountCreated { get; }

		IDistributedEvent<AccountSignedIn> AccountSignedIn { get; }
	}
}