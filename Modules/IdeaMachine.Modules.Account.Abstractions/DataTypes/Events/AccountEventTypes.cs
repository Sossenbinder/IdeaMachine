using System;
using IdeaMachine.Modules.Account.Abstractions.DataTypes.Interface;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Events
{
	public record AccountLoggedOut(IUser Session);

	public record AccountVerified(IUser OldAnonymousUser, IUser NewUser);

	public record AccountUpdateProfilePicture(Guid AccountId, string Base64ProfilePicture);

	public record AccountProfilePictureUpdated(Guid AccountId, string ProfilePictureUrl);
}