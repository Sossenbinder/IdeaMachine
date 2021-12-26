// Types
import { RegisterInfo, SignInInfo, Network } from "../types";

// Functionality
import GetRequest from "common/helper/requests/GetRequest";
import PostRequest from "common/helper/requests/PostRequest";
import MultiPartRequest from "common/helper/requests/MultiPartRequest";

const Urls = {
	ListAvailableProviders: "/SocialLogin/ListAvailableProviders"
};

export const listAvailableProviders = async () => {
	const request = new GetRequest<string[]>(Urls.ListAvailableProviders);
	return await request.get();
};
