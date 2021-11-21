using System;
using System.Threading.Tasks;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Session.Service.Interface;
using MassTransit;

namespace IdeaMachine.Modules.Account.Events
{
    public class AccountProfilePictureUpdatedConsumer : IConsumer<AccountProfilePictureUpdated>
    {
	    private readonly ISessionService _sessionService;

	    public AccountProfilePictureUpdatedConsumer(ISessionService sessionService) => _sessionService = sessionService;
		
	    public async Task Consume(ConsumeContext<AccountProfilePictureUpdated> context)
	    {
			//await _sessionService.
	    }
    }
}
