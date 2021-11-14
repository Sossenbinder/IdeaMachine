// Framework
import { useContext } from "react";

// Functionality
import { ChannelProviderContext } from "../modules/channel/ChannelProviderContext";

export const useChannelProvider = () => useContext(ChannelProviderContext);
export default useChannelProvider;