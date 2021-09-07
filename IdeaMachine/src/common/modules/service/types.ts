//Types
import { NetworkResponse } from "common/helper/requests/types/NetworkDefinitions";
import ISignalRConnectionProvider from "common/helper/signalR/interface/ISignalRConnectionProvider";
import { Idea } from "modules/ideas/types";
import { RegisterInfo, SignInInfo, IdentityErrorCode } from "modules/account/types";
import { LikeState } from "modules/reaction/types";

export interface IModuleService {
	start?(): Promise<void>;
}

export type ServiceNotification = {
	name: keyof Services;
	service: IModuleService;
}

export interface IIdeaService extends IModuleService {
	addIdea(idea: Idea, attachments?: FileList): Promise<number>;
	fetchIdeas(): Promise<void>;
	initializeOwnIdeas(): Promise<void>;
	getSpecificIdea(id: number): Promise<void>;
	deleteIdea(id: number): Promise<void>;
	deleteAttachment(ideaId: number, attachmentId: number): Promise<void>;
}

export interface IReactionService extends IModuleService {
	modifyLike(ideaId: number, likeState: LikeState): Promise<void>;
}

export interface IAccountService extends IModuleService {
	logout(): Promise<void>;
	register(registerInfo: RegisterInfo): Promise<NetworkResponse<IdentityErrorCode>>;
	login(signInInfo: SignInInfo): Promise<IdentityErrorCode>;
	verifyEmail(userName: string, token: string): Promise<NetworkResponse<IdentityErrorCode>>;
}

export type Services = {
	AccountService: IAccountService;
	IdeaService: IIdeaService;
	ReactionService: IReactionService;
	SignalRConnectionProvider: ISignalRConnectionProvider;
}