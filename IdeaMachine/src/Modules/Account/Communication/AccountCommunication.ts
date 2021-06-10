// Types
import { RegisterInfo, SignInInfo, Network } from "../types";

// Functionality
import PostRequest from "common/Helper/Requests/PostRequest"

const Urls = {
	Register: "/Account/Register",
	SignIn: "/Account/SignIn",
}

export const register = async (registerInfo: RegisterInfo) => {
	const request = new PostRequest<Network.Register.Request, Network.Register.Response>(Urls.Register);
	return await request.post(registerInfo);
}

export const signIn = async (signInInfo: SignInInfo) => {
	const request = new PostRequest<Network.SignIn.Request, void>(Urls.SignIn);
	return await request.post(signInInfo);
}