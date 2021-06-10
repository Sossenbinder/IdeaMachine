export type Account = {

}

export type SignInInfo = {
	email: string;
	password: string;
	rememberMe: boolean;
}

export type RegisterInfo = {
	email: string;
	userName: string;
	password: string;
	confirmPassword: string;
}

export enum IdentityErrorCode {
	Success = 0,
	DefaultError = 1,
	ConcurrencyFailure = 2,
	PasswordMismatch = 3,
	InvalidToken = 4,
	LoginAlreadyAssociated = 5,
	InvalidUserName = 6,
	InvalidEmail = 7,
	EmailMissing = 8,
	DuplicateUserName = 9,
	DuplicateEmail = 10,
	InvalidRoleName = 11,
	DuplicateRoleName = 12,
	UserAlreadyHasPassword = 13,
	UserLockoutNotEnabled = 14,
	UserAlreadyInRole = 15,
	UserNameNullOrEmpty = 16,
	UserNotInRole = 17,
	PasswordTooShort = 18,
	PasswordRequiresNonAlphanumeric = 19,
	PasswordRequiresDigit = 20,
	PasswordRequiresLower = 21,
	PasswordRequiresUpper = 22,
	PasswordMissing = 23,
	EmailNotConfirmed = 24,
	EmailAlreadyConfirmed = 25,
}

export namespace Network {
	export namespace Register {
		export type Request = RegisterInfo;

		export type Response = IdentityErrorCode;
	}

	export namespace SignIn {
		export type Request = SignInInfo;
	}
}