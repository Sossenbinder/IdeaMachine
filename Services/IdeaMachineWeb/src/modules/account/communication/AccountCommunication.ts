// Functionality
import MultiPartRequest from "common/helper/requests/MultiPartRequest";

const Urls = {
	GetAccount: "/Account/Get",
	Register: "/Account/Register",
	SignIn: "/Account/SignIn",
	Verify: "/Verify/VerifyMail",
	Logout: "/Account/Logout",
	GetProfilePicture: "/Profile/Picture",
	UpdateProfilePicture: "/Profile/Picture",
};

export const updateProfilePicture = async (files: FileList) => {
	const request = new MultiPartRequest<void>(Urls.UpdateProfilePicture);

	return await request.post({
		files,
	});
};
