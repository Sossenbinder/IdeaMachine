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
import { ChannelProvider, IChannelProvider } from "./common/modules/channel/ChannelProvider";

window.onload = async () => {
	await fetch("/Identity/Identify");

	const signalRConnectionProvider = new SignalRConnectionProvider();
	const channelProvider = new ChannelProvider(signalRConnectionProvider);

	renderRoot(channelProvider, () => initCoreServices(signalRConnectionProvider, channelProvider), 5);
};

const initCoreServices = async (signalRConnectionProvider: ISignalRConnectionProvider, channelProvider: IChannelProvider) => {
	await ServiceUpdateEvent.Raise({
		name: "SignalRConnectionProvider",
		service: signalRConnectionProvider,
	});

	const initPromises: Array<Promise<void>> = [];

	const initService = async (serviceName: keyof Services, service: IModuleService) => {
		await service.start();

		await ServiceUpdateEvent.Raise({
			name: serviceName,
			service: service,
		});
	};

	await signalRConnectionProvider.start();

	const ideaService = new IdeaService(channelProvider);
	initPromises.push(initService("IdeaService", ideaService));

	const commentsService = new CommentsService(channelProvider);
	initPromises.push(initService("CommentsService", commentsService));

	const accountService = new AccountService(channelProvider);
	initPromises.push(initService("AccountService", accountService));

	const reactionService = new ReactionService(channelProvider);
	initPromises.push(initService("ReactionService", reactionService));

	await Promise.all(initPromises);
};
