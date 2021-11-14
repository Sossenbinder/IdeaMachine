// Framework
import * as React from "react";
import { render } from "react-dom";
import { QueryClient, QueryClientProvider } from "react-query";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";

// Components
import LoadingBar from "common/components/state/LoadingBar";
import ServiceContextProvider from "common/modules/service/ServiceContextProvider";
import ServiceUpdateEvent from "common/modules/service/ServiceUpdateEvent";
import MainContainer from "views/main/MainContainer";
import IdeaFilterContextProvider from "modules/ideas/components/IdeaFilterContext";
import ChannelProviderContextProvider from "../common/modules/channel/ChannelProviderContext";
import AccountContextProvider from "modules/account/helper/accountContext";

// Types
import { store } from "common/redux/store";

import "./styles/RootComponent.less";
import { IChannelProvider } from "../common/modules/channel/ChannelProvider";

const queryClient = new QueryClient();

type Props = {
	channelProvider: IChannelProvider;
	initFunc(): Promise<void>;
	initServiceCount: number;
}

const RootComponent: React.FC<Props> = ({ channelProvider, initFunc, initServiceCount }) => {

	const [initialized, setInitialized] = React.useState(false);
	const [loadedServices, setLoadedServices] = React.useState(0);

	React.useEffect(() => {
		const registration = ServiceUpdateEvent.Register(() => {
			setLoadedServices(current => current + 1);
			return Promise.resolve();
		});

		initFunc().then(() => setInitialized(true));

		return () => ServiceUpdateEvent.Unregister(registration);
	}, []);

	return (
		<Provider store={store}>
			<BrowserRouter>
				<QueryClientProvider client={queryClient}>
					<ServiceContextProvider>
						<AccountContextProvider>
							<ChannelProviderContextProvider channelProvider={channelProvider}>
								<Choose>
									<When condition={loadedServices === initServiceCount && initialized}>
										<IdeaFilterContextProvider>
											<MainContainer />
										</IdeaFilterContextProvider>
									</When>
									<Otherwise>
										<LoadingBar
											progress={(loadedServices / initServiceCount) * 100} />
									</Otherwise>
								</Choose>
							</ChannelProviderContextProvider>
						</AccountContextProvider>
					</ServiceContextProvider>
				</QueryClientProvider>
			</BrowserRouter>
		</Provider>
	);
}

const renderRoot = (channelProvider: IChannelProvider, initFunc: () => Promise<void>, initServiceCount: number) => render(
	<RootComponent
		channelProvider={channelProvider}
		initFunc={initFunc}
		initServiceCount={initServiceCount} />,
	document.getElementById("reactRoot")
);

export default renderRoot;