// Framework
import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

// Functionality
import ISignalRConnectionProvider from './Interface/ISignalRConnectionProvider';

export const SignalRContext = React.createContext<HubConnection>(undefined);

export type Props = {
	signalRConnectionProvider: ISignalRConnectionProvider;
}

export const SignalRContextProvider: React.FC<Props> = ({ signalRConnectionProvider, children }) => {
	return (
		<SignalRContext.Provider value={signalRConnectionProvider.SignalRConnection}>
			{children}
		</SignalRContext.Provider>
	);
}

export default SignalRContextProvider;