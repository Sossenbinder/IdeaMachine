// Framework
import * as React from "react";
import { render } from "react-dom";
import { QueryClient, QueryClientProvider } from "react-query";
import { BrowserRouter } from "react-router-dom";
import { createMuiTheme } from '@material-ui/core/styles';
import { grey } from '@material-ui/core/colors';
import { Provider } from "react-redux";

// Components
import LoadingBar from "common/Components/State/LoadingBar";
import ServiceContextProvider from "common/Modules/Service/ServiceContextProvider";
import ServiceUpdateEvent from "common/Modules/Service/ServiceUpdateEvent";
import SignalRContextProvider from "common/Helper/SignalR/SignalRContextProvider";
import MainContainer from "views/Main/MainContainer";

// Functionality
import ISignalRConnectionProvider from 'common/Helper/SignalR/Interface/ISignalRConnectionProvider';

// Types
import { store } from "common/Redux/store";

import "./Styles/RootComponent.less";

const queryClient = new QueryClient();

type Props = {
	signalRConnectionProvider: ISignalRConnectionProvider;
	initFunc(): Promise<void>;
	initServiceCount: number;
}

const mainTheme = createMuiTheme({
	palette: {
		primary: grey,
	},
});

const RootComponent: React.FC<Props> = ({ signalRConnectionProvider, initFunc, initServiceCount }) => {

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
						<SignalRContextProvider signalRConnectionProvider={signalRConnectionProvider}>
							<Choose>
								<When condition={loadedServices === initServiceCount && initialized}>
									<MainContainer />
								</When>
								<Otherwise>
									<LoadingBar
										progress={(loadedServices / initServiceCount) * 100} />
								</Otherwise>
							</Choose>
						</SignalRContextProvider>
					</ServiceContextProvider>
				</QueryClientProvider>
			</BrowserRouter>
		</Provider>
	);
}

const renderRoot = (signalRConnectionProvider: ISignalRConnectionProvider, initFunc: () => Promise<void>, initServiceCount: number) => render(
	<RootComponent
		signalRConnectionProvider={signalRConnectionProvider}
		initFunc={initFunc}
		initServiceCount={initServiceCount} />,
	document.getElementById("reactRoot")
);

export default renderRoot;