// Types
import { Network, Comment } from "../types";

// Functionality
import PostRequest, { VoidPostRequest } from "common/helper/requests/PostRequest"

const Urls = {
	AddComment: "/Comment/Add",
    GetComments: "/Comment/GetComments",
}

export const addComment = async (comment: Comment) => {
	const request = new VoidPostRequest<Network.AddComment.Request>(Urls.AddComment);
	return await request.post(comment);
}

export const queryComments = async (ideaId: number) => {
	const request = new PostRequest<Network.QueryComments.Request, Network.QueryComments.Response>(Urls.GetComments);
	return await request.post({
        ideaId,
    });
}