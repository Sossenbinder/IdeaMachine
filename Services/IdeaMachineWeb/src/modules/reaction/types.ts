export enum LikeState {
	Neutral,
	Dislike,
	Like,x
}

export namespace Network {
	export namespace ModifyLike {
		export type Request = {
			ideaId: number;
			likeState: LikeState;
		};
	}
}