// Types
import { RegisterInfo, SignInInfo, Network } from "../types";

// Functionality
import GetRequest from "common/helper/requests/GetRequest";
import PostRequest from "common/helper/requests/PostRequest";
import MultiPartRequest from "common/helper/requests/MultiPartRequest";

const Urls = {
	GetAccount: "/Account/Get",
	Register: "/Account/Register",
	SignIn: "/Account/SignIn",
	Verify: "/Verify/VerifyMail",
	Logout: "/Account/Logout",
	UpdateProfilePicture: "/ProfilePicture/UpdateProfilePicture",
}

export const getAccount = async () => {
	const request = new GetRequest<Network.GetAccount.Response>(Urls.GetAccount);
	return await request.get();
}

export const logout = async () => {
	const request = new PostRequest(Urls.Logout);
	return await request.post();
}

export const register = async (registerInfo: RegisterInfo) => {
	const request = new PostRequest<Network.Register.Request, Network.Register.Response>(Urls.Register);
	return await request.post(registerInfo);
}

export const signIn = async (signInInfo: SignInInfo) => {
	const request = new PostRequest<Network.SignIn.Request, Network.SignIn.Response>(Urls.SignIn);
	return await request.post(signInInfo);
}

export const verifyEmail = async (userName: string, token: string) => {
	const request = new PostRequest<Network.VerifyEmail.Request, Network.VerifyEmail.Response>(Urls.Verify);
	return await request.post({
		userName,
		token,
	});
}

export const updateProfilePicture = async (files: FileList) => {
	const request = new MultiPartRequest<void>(Urls.UpdateProfilePicture);

	return await request.post({
		files,
	});
}