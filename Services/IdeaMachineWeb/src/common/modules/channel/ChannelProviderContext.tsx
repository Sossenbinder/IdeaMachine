// Framework
import * as React from "react";

// Types
import { IChannelProvider } from "./ChannelProvider";

export const ChannelProviderContext = React.createContext<IChannelProvider>(undefined);

export type Props = {
	channelProvider: IChannelProvider;
}

export const ChannelProviderContextProvider: React.FC<Props> = ({ children, channelProvider }) => {
	return (
		<ChannelProviderContext.Provider value={channelProvider}>
			{children}
		</ChannelProviderContext.Provider>
	);
}

export default ChannelProviderContextProvider;