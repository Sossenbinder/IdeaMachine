// Framework
import * as React from "react";

// Functionality
import ServiceContext from "./ServiceContext";
import ServiceUpdateEvent from "./ServiceUpdateEvent";

// Types
import { Services } from "./types";

export const ServiceContextProvider: React.FC = ({ children }) => {

	const [serviceState, setServiceState] = React.useState<Services>({} as Services);

	React.useEffect(() => {
		const localServiceState = {} as Services;

		const listenerId = ServiceUpdateEvent.Register(notification => {
			localServiceState[notification.name] = notification.service as any;
			setServiceState({ ...localServiceState });
			return Promise.resolve();
		});

		return () => ServiceUpdateEvent.Unregister(listenerId);
	}, []);

	return <ServiceContext.Provider value={serviceState}>{children}</ServiceContext.Provider>;
}

export default ServiceContextProvider;