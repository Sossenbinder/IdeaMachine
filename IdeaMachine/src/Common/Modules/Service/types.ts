//Types
import { NetworkResponse } from "Common/Helper/Requests/Types/NetworkDefinitions";
import ISignalRConnectionProvider from "common/Helper/SignalR/Interface/ISignalRConnectionProvider";
import { Idea } from "modules/Ideas/types";
import { RegisterInfo, SignInInfo, IdentityErrorCode } from "modules/Account/types";

export interface IModuleService {
	start?(): Promise<void>;
}

export type ServiceNotification = {
	name: keyof Services;
	service: IModuleService;
}

export interface IIdeaService extends IModuleService {
	addIdea(idea: Idea): Promise<void>;
	initializeIdeas(): Promise<void>;
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
	SignalRConnectionProvider: ISignalRConnectionProvider;
}