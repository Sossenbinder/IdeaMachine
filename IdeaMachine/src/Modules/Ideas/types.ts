export type Idea = {
	id: number;
	shortDescription: string;
	longDescription: string;
	creatorId: string;
	creationDate: Date;
	ideaReactionMetaData: IdeaReactionMetaData;
}

export type IdeaReactionMetaData = {
	ownLikeState: number;
	totalLike: number;
}

export namespace Network {
	export namespace Add {
		export type Request = Idea;
	}

	export namespace Get {
		export type Response = Idea;
	}

	export namespace GetForUser {
		export type Request = string;
		export type Response = Array<Idea>;
	}

	export namespace GetSpecificIdea {
		export type Request = number;
		export type Response = Idea | undefined;
	}

	export namespace Reply {
		export type Request = {

		}
	}
}