import { Comment } from "modules/comments/types";

export type Idea = {
	id: number;
	shortDescription: string;
	longDescription: string;
	creatorId: string;
	creationDate: Date;
	tags: Array<string>;
	attachmentUrls: Array<AttachmentUrl>;
	ideaReactionMetaData: IdeaReactionMetaData;
	comments?: Array<Comment>;
};

export type AttachmentUrl = {
	id: number;
	attachmentUrl: string;
};

export type IdeaReactionMetaData = {
	ownLikeState: number;
	totalLike: number;
};

export enum OrderType {
	Created,
	Description,
	Popularity,
}

export enum OrderDirection {
	Up,
	Down,
}

export enum IdeaDeleteErrorCode {
	UnspecifiedError,
	Successful,
	NotOwned,
	NotFound,
}

export enum IdeaInputResult {
	UnspecifiedError = 0,
	Successful = 1 << 0,
	MissingShortDescription = 1 << 1,
	MissingLongDescription = 1 << 2,
}

export type LikeCommitedNotification = {
	ideaId: string;
	success: boolean;
};

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

	export namespace Delete {
		export type Request = number;
		export type Response = IdeaDeleteErrorCode;
	}

	export namespace UploadAttachment {
		export type Request = {
			ideaId: number;
		};
		export type Response = number;
	}

	export namespace DeleteAttachment {
		export type Request = {
			attachmentId: number;
			ideaId: number;
		};
	}

	export namespace Reply {
		export type Request = {};
	}
}
