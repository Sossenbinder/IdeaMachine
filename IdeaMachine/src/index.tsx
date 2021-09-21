// Components
import renderRoot from "views/RootComponent";

// Functionality
import ServiceUpdateEvent from "common/modules/service/ServiceUpdateEvent";
import ISignalRConnectionProvider from "common/helper/signalR/interface/ISignalRConnectionProvider";
import SignalRConnectionProvider from "common/helper/signalR/SignalRConnectionProvider";
import IdeaService from "modules/ideas/service/IdeaService";
import AccountService from "modules/account/service/AccountService";
import ReactionService from "modules/reaction/service/ReactionService";
import CommentsService from "modules/comments/service/commentsService";

// Types
import { Services, IModuleService } from "common/modules/service/types";

window.onload = async () => {

	await fetch("/Identity/Identify");

	const signalRConnectionProvider = new SignalRConnectionProvider();

	renderRoot(signalRConnectionProvider, () => initCoreServices(signalRConnectionProvider), 5);
}

const initCoreServices = async (signalRProvider: ISignalRConnectionProvider) => {

	await ServiceUpdateEvent.Raise({
		name: "SignalRConnectionProvider",
		service: signalRProvider,
	});

	const initPromises: Array<Promise<void>> = [];

	const initService = async (serviceName: keyof Services, service: IModuleService) => {
		await service.start();

		await ServiceUpdateEvent.Raise({
			name: serviceName,
			service: service,
		});
	}

	await signalRProvider.start();

	const ideaService = new IdeaService();
	initPromises.push(initService("IdeaService", ideaService));

	const commentsService = new CommentsService();
	initPromises.push(initService("CommentsService", commentsService));

	const accountService = new AccountService();
	initPromises.push(initService("AccountService", accountService));

	const reactionService = new ReactionService();
	initPromises.push(initService("ReactionService", reactionService));

	await Promise.all(initPromises);
}