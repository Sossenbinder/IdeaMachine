using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Common.AspNetIdentity.DataTypes
{
	public class CodeIdentityError : IdentityError
	{
		public IdentityErrorCode ErrorCode
		{
			get;
			set;
		}
	}
}