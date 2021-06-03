//Types
import ISignalRConnectionProvider from "common/Helper/SignalR/Interface/ISignalRConnectionProvider";
import { Idea } from 'modules/Ideas/types';

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

export type Services = {
	IdeaService: IIdeaService;
	SignalRConnectionProvider: ISignalRConnectionProvider;
}