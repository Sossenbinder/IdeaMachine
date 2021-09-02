// Types
import { Idea, Network } from "../types";

// Functionality
import PostRequest, { PagedPostRequest, VoidPostRequest } from "common/helper/Requests/PostRequest";
import GetRequest from "common/helper/Requests/GetRequest";
import DeleteRequest from "common/helper/Requests/DeleteRequest";
import MultiPartRequest from "common/helper/Requests/MultiPartRequest";

const Urls = {
	Add: "/Idea/Add",
	Get: "/Idea/Get",
	GetOwn: "/Idea/GetOwn",
	GetForUser: "/Idea/GetForUser",
	GetSpecific: "/Idea/GetSpecificIdea",
	Delete: "/Idea/Delete",
	Reply: "/Idea/Reply",
}

export const postIdea = async (idea: Idea, files?: FileList) => {
	const request = new MultiPartRequest<Network.Add.Request, void>(Urls.Add);
	return await request.post({
		files: files ?? new FileList(),
		data: idea,
	});
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

export const deleteIdea = async (id: number) => {
	const request = new DeleteRequest<Network.Delete.Request, Network.Delete.Response>(Urls.Delete);
	return await request.delete(id);
}

export const reply = async () => {
	const request = new VoidPostRequest<Network.Reply.Request>(Urls.Reply);
	return await request.post();
}