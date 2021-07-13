// Types
import { Network, LikeState } from "../types";

// Functionality
import { VoidPostRequest } from "common/Helper/Requests/PostRequest"

const Urls = {
	ModifyLike: "/Reaction/ModifyLike"
}

export const modifyLike = async (ideaId: number, likeState: LikeState) => {
	const request = new VoidPostRequest<Network.ModifyLike.Request>(Urls.ModifyLike);
	return await request.post({
		ideaId,
		likeState,
	});
}