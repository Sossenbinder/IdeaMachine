// Functionality
import { IReactionService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as reactionCommunication from "modules/reaction/communication/ReactionCommunication";
import { reducer as ideaReducer } from "modules/ideas/reducer/IdeaReducer";

// Types
import ISignalRConnectionProvider from "common/helper/signalR/interface/ISignalRConnectionProvider";
import { LikeState } from "../types";

export default class ReactionService extends ModuleService implements IReactionService {

	public constructor(signalRConnectionProvider: ISignalRConnectionProvider) {
		super(signalRConnectionProvider);
	}

	public start() {
		return Promise.resolve();
	}

	modifyLike = async (ideaId: number, likeState: LikeState) => {

		const likedIdea = this.getStore().ideaReducer.data.find(x => x.id === ideaId);

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
						newTotalLikeCount += (likeState === LikeState.Like ? 1 : -1);
						break;
					case LikeState.Like:
						newTotalLikeCount += (likeState === LikeState.Dislike ? -2 : -1);
						break;
					case LikeState.Dislike:
						newTotalLikeCount += (likeState === LikeState.Like ? 2 : 1);
						break;
				}

				this.dispatch(ideaReducer.put({
					...likedIdea,
					ideaReactionMetaData: {
						totalLike: newTotalLikeCount,
						ownLikeState: likeState
					}
				}));
			}
		}
	}
}
