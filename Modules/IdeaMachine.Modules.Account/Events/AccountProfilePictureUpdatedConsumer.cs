using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using MassTransit;

namespace IdeaMachine.Modules.Account.Events
{
    public class AccountProfilePictureUpdatedConsumer : IConsumer<AccountProfilePictureUpdated>
    {
	    public Task Consume(ConsumeContext<AccountProfilePictureUpdated> context)
	    {
		    throw new NotImplementedException();
	    }
    }
}
