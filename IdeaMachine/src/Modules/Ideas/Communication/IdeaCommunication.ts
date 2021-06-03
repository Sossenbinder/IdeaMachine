// Types
import { Idea, Network } from "../types";

// Functionality
import PostRequest from "common/Helper/Requests/PostRequest"
import GetRequest from 'common/Helper/Requests/GetRequest';

const Urls = {
	Add: "Idea/Add",
	Get: "Idea/Get",
}

export const postIdea = async (idea: Idea) => {
	const request = new PostRequest<Network.Add.Request, void>(Urls.Add);
	return await request.post(idea);
}

export const getIdeas = async () => {
	const request = new GetRequest<Network.Get.Response>(Urls.Get);
	return await request.get();
}