using System.Collections.Generic;
using System.Linq;
using IdeaMachine.Common.AspNetIdentity.DataTypes;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Common.AspNetIdentity.Extension
{
	public static class IdentityResultExtensions
	{
		public static IdentityErrorCode? FirstErrorOrNull(this IdentityResult identityResult)
		{
			var errorCodes = identityResult
				.GetErrorCodes()
				.ToList();

			return errorCodes.Any() ? errorCodes.FirstOrDefault() : null;
		}

		public static IdentityErrorCode FirstErrorOrFail(this IdentityResult identityResult)
		{
			var errorCodes = identityResult
				.GetErrorCodes()
				.ToList();

			return errorCodes.First();
		}

		public static IdentityErrorCode FirstErrorOrDefault(this IdentityResult identityResult)
		{
			var errorCode = identityResult.FirstErrorOrNull();

			return errorCode ?? IdentityErrorCode.DefaultError;
		}

		public static IEnumerable<IdentityErrorCode> GetErrorCodes(this IdentityResult identityResult)
		{
			var errorCodes = identityResult
				.Errors
				.OfType<CodeIdentityError>()
				.Select(x => x.ErrorCode);

			return errorCodes;
		}
	}
}