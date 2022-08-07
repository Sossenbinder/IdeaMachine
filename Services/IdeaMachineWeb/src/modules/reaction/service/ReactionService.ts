// Functionality
import { IReactionService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as reactionCommunication from "modules/reaction/communication/ReactionCommunication";
import { reducer as ideaReducer } from "modules/ideas/reducer/IdeaReducer";

// Types
import { IChannelProvider } from "common/modules/channel/ChannelProvider";
import { LikeState } from "../types";
import { LikeCommitedFailedInfo } from "../../ideas/types";
import BackendNotification from "common/helper/signalR/Notifications";
import { Notification } from "common/helper/signalR/types";

export default class ReactionService extends ModuleService implements IReactionService {
	public constructor(channelProvider: IChannelProvider) {
		super(channelProvider);
	}

	public async start() {
		this.ChannelProvider.getBackendChannel<LikeCommitedFailedInfo>(BackendNotification.LikeCommitFailed).register(this.onLikeCommittedNotification);
	}

	private onLikeCommittedNotification = async ({ payload: { ideaId, rollbackState } }: Notification<LikeCommitedFailedInfo>) => {
		const likedIdea = this.getStore().ideaReducer.data.find((x) => x.id === ideaId);

		const { totalLike: newTotalLikeCount } = likedIdea.ideaReactionMetaData;

		this.dispatch(
			ideaReducer.put({
				...likedIdea,
				ideaReactionMetaData: {
					totalLike: newTotalLikeCount + (rollbackState === LikeState.Like ? -1 : rollbackState === LikeState.Dislike ? 1 : 0),
					ownLikeState: rollbackState,
				},
			}),
		);
	};

	modifyLike = async (ideaId: number, likeState: LikeState) => {
		const likedIdea = this.getStore().ideaReducer.data.find((x) => x.id === ideaId);

		if (likeState == likedIdea.ideaReactionMetaData.ownLikeState) {
			likeState = LikeState.Neutral;
		}

		const ownLikeState = likedIdea.ideaReactionMetaData.ownLikeState;

		const response = await reactionCommunication.modifyLike(ideaId, likeState);

		if (response.success) {
			if (likedIdea) {
				let { totalLike: newTotalLikeCount } = likedIdea.ideaReactionMetaData;

				switch (ownLikeState) {
					case LikeState.Neutral:
						newTotalLikeCount += likeState === LikeState.Like ? 1 : -1;
						break;
					case LikeState.Like:
						newTotalLikeCount += likeState === LikeState.Dislike ? -2 : -1;
						break;
					case LikeState.Dislike:
						newTotalLikeCount += likeState === LikeState.Like ? 2 : 1;
						break;
				}

				this.dispatch(
					ideaReducer.put({
						...likedIdea,
						ideaReactionMetaData: {
							totalLike: newTotalLikeCount,
							ownLikeState: likeState,
						},
					}),
				);
			}
		}
	};
}
