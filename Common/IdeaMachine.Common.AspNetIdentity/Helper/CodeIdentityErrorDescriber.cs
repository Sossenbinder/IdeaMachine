using IdeaMachine.Common.AspNetIdentity.DataTypes;
using Microsoft.AspNetCore.Identity;

namespace IdeaMachine.Common.AspNetIdentity.Helper
{
	public class CodeIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DefaultError()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.DefaultError,
				Code = nameof(DefaultError),
			};
		}

		public override IdentityError ConcurrencyFailure()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.ConcurrencyFailure,
				Code = nameof(ConcurrencyFailure),
			};
		}

		public override IdentityError PasswordMismatch()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.PasswordMismatch,
				Code = nameof(PasswordMismatch),
			};
		}

		public override IdentityError InvalidToken()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.InvalidToken,
				Code = nameof(InvalidToken),
			};
		}

		public override IdentityError LoginAlreadyAssociated()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.LoginAlreadyAssociated,
				Code = nameof(LoginAlreadyAssociated),
			};
		}

		public override IdentityError InvalidUserName(string userName)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.InvalidUserName,
				Code = nameof(InvalidUserName),
			};
		}

		public override IdentityError InvalidEmail(string email)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.InvalidEmail,
				Code = nameof(InvalidEmail),
			};
		}

		public override IdentityError DuplicateUserName(string userName)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.DuplicateUserName,
				Code = nameof(DuplicateUserName),
			};
		}

		public override IdentityError DuplicateEmail(string email)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.DuplicateEmail,
				Code = nameof(DuplicateEmail),
			};
		}

		public override IdentityError InvalidRoleName(string role)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.InvalidRoleName,
				Code = nameof(InvalidRoleName),
			};
		}

		public override IdentityError DuplicateRoleName(string role)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.DuplicateRoleName,
				Code = nameof(DuplicateRoleName),
			};
		}

		public override IdentityError UserAlreadyHasPassword()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.UserAlreadyHasPassword,
				Code = nameof(UserAlreadyHasPassword),
			};
		}

		public override IdentityError UserLockoutNotEnabled()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.UserLockoutNotEnabled,
				Code = nameof(UserLockoutNotEnabled),
			};
		}

		public override IdentityError UserAlreadyInRole(string role)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.UserAlreadyInRole,
				Code = nameof(UserAlreadyInRole),
			};
		}

		public override IdentityError UserNotInRole(string role)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.UserNotInRole,
				Code = nameof(UserNotInRole),
			};
		}

		public override IdentityError PasswordTooShort(int length)
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.PasswordTooShort,
				Code = nameof(PasswordTooShort),
			};
		}

		public override IdentityError PasswordRequiresNonAlphanumeric()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.PasswordRequiresNonAlphanumeric,
				Code = nameof(PasswordRequiresNonAlphanumeric),
			};
		}

		public override IdentityError PasswordRequiresDigit()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.PasswordRequiresDigit,
				Code = nameof(PasswordRequiresDigit),
			};
		}

		public override IdentityError PasswordRequiresLower()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.PasswordRequiresLower,
				Code = nameof(PasswordRequiresLower),
			};
		}

		public override IdentityError PasswordRequiresUpper()
		{
			return new CodeIdentityError
			{
				ErrorCode = IdentityErrorCode.PasswordRequiresUpper,
				Code = nameof(PasswordRequiresUpper),
			};
		}
	}
}