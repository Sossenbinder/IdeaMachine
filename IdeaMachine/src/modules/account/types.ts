export type Account = {
	isAnonymous: boolean;
	userName: string;
	email: string;
	lastAccessedAt: Date;
	userId: string;
}

export type SignInInfo = {
	emailUserName: string;
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
	DefaultError = 0,
	Success = 1,
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
	InvalidEmailOrUserName = 26,
}

export namespace Network {
	export namespace GetAccount {
		export type Response = Account;
	}

	export namespace Register {
		export type Request = RegisterInfo;

		export type Response = IdentityErrorCode;
	}

	export namespace SignIn {
		export type Request = SignInInfo;

		export type Response = IdentityErrorCode;
	}

	export namespace VerifyEmail {
		export type Request = {
			userName: string;
			token: string;
		}

		export type Response = IdentityErrorCode;
	}
}