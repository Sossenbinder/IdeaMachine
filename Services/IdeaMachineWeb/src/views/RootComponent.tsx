// Framework
import * as React from "react";
import { render } from "react-dom";
import { QueryClient, QueryClientProvider } from "react-query";
import { BrowserRouter } from "react-router-dom";
import { createTheme, ThemeOptions, ThemeProvider } from "@mui/material/styles";
import { Provider } from "react-redux";
import { MsalProvider } from "@azure/msal-react";

// Components
import LoadingBar from "common/components/state/LoadingBar";
import ServiceContextProvider from "common/modules/service/ServiceContextProvider";
import ServiceUpdateEvent from "common/modules/service/ServiceUpdateEvent";
import MainContainer from "views/main/MainContainer";
import IdeaFilterContextProvider from "modules/ideas/components/IdeaFilterContext";
import ChannelProviderContextProvider from "../common/modules/channel/ChannelProviderContext";
import AccountContextProvider from "modules/account/helper/accountContext";
import { IChannelProvider } from "../common/modules/channel/ChannelProvider";

// Types
import { store } from "common/redux/store";

import "./styles/RootComponent.scss";
import { ColorScheme, ColorSchemeProvider, MantineProvider } from "@mantine/styles";
import { customStyles } from "./styles/mantineStyles";
import { msalInstance } from "../modules/account/msal/msalConfig";
import mantineTheme from "views/styles/mantineTheme";

const queryClient = new QueryClient();

type Props = {
	channelProvider: IChannelProvider;
	initFunc(): Promise<void>;
	initServiceCount: number;
};

const RootComponent: React.FC<Props> = ({ channelProvider, initFunc, initServiceCount }) => {
	const [initialized, setInitialized] = React.useState(false);
	const [loadedServices, setLoadedServices] = React.useState(0);

	React.useEffect(() => {
		const registration = ServiceUpdateEvent.register(() => {
			setLoadedServices((current) => current + 1);
			return Promise.resolve();
		});

		initFunc().then(() => setInitialized(true));

		return () => ServiceUpdateEvent.unregister(registration);
	}, []);

	const themeOptions: ThemeOptions = React.useMemo(
		() =>
			createTheme({
				palette: {
					primary: {
						main: "#3f51b5",
					},
					secondary: {
						main: "#928b8e",
					},
				},
			}),
		[],
	);

	const [currentColorScheme, setCurrentColorScheme] = React.useState<ColorScheme>("dark");
	const toggleColorScheme = (value?: ColorScheme) => setCurrentColorScheme(!value || currentColorScheme === "dark" ? "light" : "dark");

	return (
		<Provider store={store}>
			<MsalProvider instance={msalInstance}>
				<BrowserRouter>
					<ThemeProvider theme={themeOptions}>
						<MantineProvider
							withGlobalStyles
							withNormalizeCSS
							styles={customStyles}
							theme={{
								...mantineTheme,
								colorScheme: currentColorScheme,
							}}
						>
							<ColorSchemeProvider colorScheme={currentColorScheme} toggleColorScheme={toggleColorScheme}>
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
														<LoadingBar progress={(loadedServices / initServiceCount) * 100} />
													</Otherwise>
												</Choose>
											</ChannelProviderContextProvider>
										</AccountContextProvider>
									</ServiceContextProvider>
								</QueryClientProvider>
							</ColorSchemeProvider>
						</MantineProvider>
					</ThemeProvider>
				</BrowserRouter>
			</MsalProvider>
		</Provider>
	);
};

const renderRoot = (channelProvider: IChannelProvider, initFunc: () => Promise<void>, initServiceCount: number) =>
	render(<RootComponent channelProvider={channelProvider} initFunc={initFunc} initServiceCount={initServiceCount} />, document.getElementById("reactRoot"));

export default renderRoot;
