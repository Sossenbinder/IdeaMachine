//Types
import ISignalRConnectionProvider from "common/helper/signalR/interface/ISignalRConnectionProvider";
import { Idea } from "modules/ideas/types";
import { LikeState } from "modules/reaction/types";

export interface IModuleService {
	start?(): Promise<void>;
}

export type ServiceNotification = {
	name: keyof Services;
	service: IModuleService;
};

export interface IIdeaService extends IModuleService {
	addIdea(idea: Idea, attachments?: Array<File>): Promise<number>;
	countMine(): Promise<number>;
	countAll(): Promise<number>;
	fetchIdeas(): Promise<void>;
	initializeOwnIdeas(): Promise<void>;
	getSpecificIdea(id: number): Promise<void>;
	deleteIdea(id: number): Promise<void>;
	uploadAttachment(ideaId: number, file: File): Promise<void>;
	deleteAttachment(ideaId: number, attachmentId: number): Promise<void>;
}

export interface IReactionService extends IModuleService {
	modifyLike(ideaId: number, likeState: LikeState): Promise<void>;
}

export interface ICommentsService extends IModuleService {
	addComment(ideaId: number, comment: string): Promise<boolean>;
	queryComments(ideaId: number): Promise<void>;
}

export interface IAccountService extends IModuleService {
	login(): Promise<void>;
}

export type Services = {
	AccountService: IAccountService;
	CommentsService: ICommentsService;
	IdeaService: IIdeaService;
	ReactionService: IReactionService;
	SignalRConnectionProvider: ISignalRConnectionProvider;
};
