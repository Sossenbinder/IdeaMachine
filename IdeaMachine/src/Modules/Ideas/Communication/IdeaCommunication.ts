// Types
import { Idea, Network } from "../types";

// Functionality
import PostRequest, { PagedPostRequest, VoidPostRequest } from "common/Helper/Requests/PostRequest"
import GetRequest from 'common/Helper/Requests/GetRequest';

const Urls = {
	Add: "/Idea/Add",
	Get: "/Idea/Get",
	GetOwn: "/Idea/GetOwn",
	GetForUser: "/Idea/GetForUser",
	GetSpecific: "/Idea/GetSpecificIdea",
	Reply: "/Idea/Reply",
}

export const postIdea = async (idea: Idea) => {
	const request = new PostRequest<Network.Add.Request, void>(Urls.Add);
	return await request.post(idea);
}

export const getIdeas = async (paginationToken: number | null = null) => {
	const request = new PagedPostRequest<Network.Get.Response, number>(Urls.Get);
	return await request.post(paginationToken);
}

export const getOwnIdeas = async () => {
	const request = new GetRequest<Network.GetForUser.Response>(Urls.GetOwn);
	return await request.get();
}

export const getIdeasForUser = async (userId: string) => {
	const request = new PostRequest<Network.GetForUser.Request, Network.GetForUser.Response>(Urls.GetForUser);
	return await request.post(userId);
}

export const getSpecificIdea = async (id: number) => {
	const request = new PostRequest<Network.GetSpecificIdea.Request, Network.GetSpecificIdea.Response>(Urls.GetSpecific);
	return await request.post(id);
}

export const reply = async () => {
	const request = new VoidPostRequest<Network.Reply.Request>(Urls.Reply);
	return await request.post();
}