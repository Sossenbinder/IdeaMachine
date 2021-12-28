// Functionality
import GetRequest from "common/helper/requests/GetRequest";

const Urls = {
	ListAvailableProviders: "/SocialLogin/ListAvailableProviders"
};

export const listAvailableProviders = async () => {
	const request = new GetRequest<string[]>(Urls.ListAvailableProviders);
	return await request.get();
};
