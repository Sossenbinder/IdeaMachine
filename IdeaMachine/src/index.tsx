// Components
import renderRoot from "views/RootComponent";

// Functionality
import ServiceUpdateEvent from "common/Modules/Service/ServiceUpdateEvent";
import ISignalRConnectionProvider from "common/Helper/SignalR/Interface/ISignalRConnectionProvider";
import SignalRConnectionProvider from "common/Helper/SignalR/SignalRConnectionProvider";
import IdeaService from "modules/Ideas/Service/IdeaService";

// Types
import { Services, IModuleService } from "common/Modules/Service/types";

window.onload = async () => {
	const signalRConnectionProvider = new SignalRConnectionProvider();

	renderRoot(signalRConnectionProvider, () => initCoreServices(signalRConnectionProvider), 2);
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

	await Promise.all(initPromises);
}