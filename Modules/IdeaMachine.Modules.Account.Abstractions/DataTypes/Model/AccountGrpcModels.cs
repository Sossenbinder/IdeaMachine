using System;

namespace IdeaMachine.Modules.Account.Abstractions.DataTypes.Model
{
	public class GetAccountName
	{
		public record Request(Guid UserIdentifier);

		public record Response(string? UserName);
	}
}
