export type TL = {
	AddIdea: string;
	AddIdeaShortDescription: "Short description of your idea";
	AddIdeaLongDescription: string;
	AddIdeaYourEmail: string;
	Home: "Home";
	GenericError: "Error";
	InputMissing: "Input missing";
	SocialLoginErrorHeading: "Something went wrong";
	SocialLoginError_Unknown: "Sadly we don't quite know what happened - Feel free to try again.";
	SocialLoginError_InfoUnavailable: "It seems like your login partner did not properly forward the required info to us.";
	SocialLoginError_EmailNotKnown: "We could not determine your email from what the login partner forwarded to us.";
	SocialLoginError_CouldntCreateAccount: "We couldn't find an existing account for this email, and were also not able to create a new one for you.";

	[key: string]: string;
};

export default TL;
