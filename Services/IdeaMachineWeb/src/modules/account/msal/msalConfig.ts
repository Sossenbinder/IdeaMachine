import { Configuration, LogLevel, PublicClientApplication } from "@azure/msal-browser";

export const scopes = {
	user: "https://ideamachineapp.onmicrosoft.com/c8be35cf-b545-4588-bb12-12766342b3ef/user",
};

export const loginRequest = {
	scopes: [scopes.user],
};

export const msalConfig: Configuration = {
	auth: {
		clientId: "c8be35cf-b545-4588-bb12-12766342b3ef", // This is the ONLY mandatory field that you need to supply.
		redirectUri: "https://localhost:21534/", // Points to window.location.origin. You must register this URI on Azure Portal/App Registration.
		postLogoutRedirectUri: "https://localhost:21534/", // Indicates the page to navigate after logout.
		navigateToLoginRequestUrl: false, // If "true", will navigate back to the original request location before processing the auth code response.
		authority: "https://ideamachineapp.b2clogin.com/ideamachineapp.onmicrosoft.com/B2C_1_ideamachineapp_signupsignin",
		knownAuthorities: ["https://ideamachineapp.b2clogin.com"],
	},
	cache: {
		cacheLocation: "localStorage", // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
		storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
	},
	system: {
		loggerOptions: {
			loggerCallback: (_, message, __) => console.log(message),
			logLevel: LogLevel.Error,
		},
	},
};

export const msalInstance = new PublicClientApplication(msalConfig);
