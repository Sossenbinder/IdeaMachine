// Functionality
import { ICommentsService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as commentsCommunication from "modules/comments/communication/commentsCommunication";
import { reducer as ideaReducer } from "modules/ideas/reducer/IdeaReducer";

// Types
import { Comment } from "../types";
import { CouldBeArray } from "../../../common/types/arrayTypes";
import { ensureArray } from "../../../common/helper/arrayUtils";

export default class CommentsService extends ModuleService implements ICommentsService {

	public constructor() {
		super();
	}

	public start() {
		return Promise.resolve();
	}

	public async addComment(ideaId: number, comment: string) {
		await commentsCommunication.addComment({
			comment,
			ideaId,
		} as Comment);
	}

	public async queryComments(ideaId: number) {
		const response = await commentsCommunication.queryComments(ideaId);

		if (response.success) {
			this.addCommentsToRedux(ideaId, response.payload);
		}
	}

	private addCommentsToRedux(ideaId: number, comments: CouldBeArray<Comment>) {
		const idea = this.getStore().ideaReducer.data.find(x => x.id === ideaId)

		ideaReducer.put({
			...idea,
			comments: [ ...idea.comments, ...ensureArray(comments)],
		});
	}
}
