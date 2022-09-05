// Functionality
import { ICommentsService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as commentsCommunication from "modules/comments/communication/commentsCommunication";
import { reducer as ideaReducer } from "modules/ideas/reducer/IdeaReducer";

// Functionality
import { ensureArray } from "common/helper/arrayUtils";

// Types
import { Comment } from "../types";
import { CouldBeArray } from "common/types/arrayTypes";
import BackendNotification from "common/helper/signalR/Notifications";
import { Notification, Operation } from "common/helper/signalR/types";
import { IChannelProvider } from "common/modules/channel/ChannelProvider";

export default class CommentsService extends ModuleService implements ICommentsService {
	public constructor(channelProvider: IChannelProvider) {
		super(channelProvider);
	}

	public start() {
		this.ChannelProvider.getBackendChannel(BackendNotification.Comment).register(this.onCommentNotification);

		return Promise.resolve();
	}

	private onCommentNotification = ({ operation, payload }: Notification<Comment>) => {
		switch (operation) {
			case Operation.Create:
				this.addCommentsToRedux(payload.ideaId, payload);
		}

		return Promise.resolve();
	};

	public async addComment(ideaId: number, comment: string) {
		const commentEntity = {
			comment,
			ideaId,
		} as Comment;

		const response = await commentsCommunication.addComment(commentEntity);

		if (response.success) {
			this.addCommentsToRedux(ideaId, commentEntity);
		}

		return response.success;
	}

	public async queryComments(ideaId: number) {
		const response = await commentsCommunication.queryComments(ideaId);

		if (response.success) {
			this.addCommentsToRedux(ideaId, response.payload);
		}
	}

	private addCommentsToRedux(ideaId: number, comments: CouldBeArray<Comment>) {
		const store = this.getStore();

		const idea = store.ideaReducer.data.find((x) => x.id === ideaId);
		const name = store.accountReducer.data.userName;

		this.dispatch(
			ideaReducer.put({
				...idea,
				comments: [...(idea.comments ?? []), ...ensureArray(comments)].map((x) => ({
					...x,
					commenterName: name,
				})),
			}),
		);
	}
}
