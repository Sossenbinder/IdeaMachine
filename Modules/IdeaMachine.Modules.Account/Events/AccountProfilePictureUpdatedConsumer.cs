using System;
using System.Threading.Tasks;
using IdeaMachine.Common.Eventing.DataTypes;
using IdeaMachine.Common.Eventing.Helper;
using IdeaMachine.Common.Eventing.MassTransit.Service.Interface;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Events;
using IdeaMachine.Modules.Session.Service.Interface;
using MassTransit;

namespace IdeaMachine.Modules.Account.Events
{
    public class AccountProfilePictureUpdatedConsumer : IConsumer<AccountProfilePictureUpdated>
    {
	    private readonly ISessionService _sessionService;

	    private readonly INotificationService _notificationService;

	    public AccountProfilePictureUpdatedConsumer(
		    ISessionService sessionService, 
		    INotificationService notificationService)
	    {
		    _sessionService = sessionService;
		    _notificationService = notificationService;
	    }

	    public async Task Consume(ConsumeContext<AccountProfilePictureUpdated> context)
	    {
		    var (accountId, profilePictureUrl) = context.Message;

		    if (_sessionService.GetSession(accountId) is null)
		    {
			    return;
		    }

		    await _sessionService.UpdateSession(accountId, session =>
		    {
			    session.User.ProfilePictureUrl = profilePictureUrl;
		    });
			
			await _notificationService.RaiseForGroup(accountId.ToString(), NotificationFactory.Update(_sessionService.GetSession(accountId)!.User, NotificationType.UserDetails));
		}
    }
}
