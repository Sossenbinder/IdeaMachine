// Types
import { Idea, Network } from "../types";

// Functionality
import PostRequest, { PagedPostRequest, VoidPostRequest } from "common/helper/requests/PostRequest";
import GetRequest from "common/helper/requests/GetRequest";
import DeleteRequest from "common/helper/requests/DeleteRequest";
import MultiPartRequest from "common/helper/requests/MultiPartRequest";

const Urls = {
	Add: "/Idea/Add",
	Get: "/Idea",
	GetOwn: "/Idea/GetOwn",
	GetForUser: "/Idea/GetForUser",
	GetSpecific: "/Idea/GetSpecificIdea",
	Delete: "/Idea/Delete",
	UploadAttachment: "/Attachment/Upload",
	DeleteAttachment: "/Attachment/Delete",
	Reply: "/Idea/Reply",
};

export const postIdea = async (idea: Idea, files?: FileList) => {
	const request = new MultiPartRequest<Network.Add.Request, void>(Urls.Add);
	return await request.post({
		files: files ?? new FileList(),
		data: idea,
	});
};

export const getIdeas = async (paginationToken: number | null = null) => {
	const request = new PagedPostRequest<Network.Get.Response, number>(Urls.Get);
	return await request.post(paginationToken);
};

export const getOwnIdeas = async () => {
	const request = new GetRequest<Network.GetForUser.Response>(Urls.GetOwn);
	return await request.get();
};

export const getIdeasForUser = async (userId: string) => {
	const request = new PostRequest<Network.GetForUser.Request, Network.GetForUser.Response>(Urls.GetForUser);
	return await request.post(userId);
};

export const getSpecificIdea = async (id: number) => {
	const request = new PostRequest<Network.GetSpecificIdea.Request, Network.GetSpecificIdea.Response>(Urls.GetSpecific);
	return await request.post(id);
};

export const deleteIdea = async (id: number) => {
	const request = new DeleteRequest<Network.Delete.Request, Network.Delete.Response>(Urls.Delete);
	return await request.delete(id);
};

export const uploadAttachment = async (ideaId: number, file: File) => {
	const request = new MultiPartRequest<Network.UploadAttachment.Request, Network.UploadAttachment.Response>(Urls.UploadAttachment);

	return await request.post({
		data: {
			ideaId,
		},
		files: [file],
	});
};

export const deleteAttachment = async (ideaId: number, attachmentId: number) => {
	const request = new DeleteRequest<Network.DeleteAttachment.Request>(Urls.DeleteAttachment);
	return await request.delete({
		attachmentId,
		ideaId,
	});
};

export const reply = async () => {
	const request = new VoidPostRequest<Network.Reply.Request>(Urls.Reply);
	return await request.post();
};
