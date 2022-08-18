// Functionality
import MultiPartRequest from "common/helper/requests/MultiPartRequest";

const Urls = {
	GetAccount: "/Account/Get",
	Register: "/Account/Register",
	SignIn: "/Account/SignIn",
	Verify: "/Verify/VerifyMail",
	Logout: "/Account/Logout",
	GetProfilePicture: "/ProfilePicture",
	UpdateProfilePicture: "/ProfilePicture",
};

export const updateProfilePicture = async (files: FileList) => {
	const request = new MultiPartRequest<void>(Urls.UpdateProfilePicture);

	return await request.post({
		files,
	});
};
