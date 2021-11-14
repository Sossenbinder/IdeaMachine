// Functionality
import useChannelProvider from "./useChannelProvider";

// Types
import BackendNotification from "../helper/signalR/Notifications";

export const useBackendChannel = <T>(notificationType: BackendNotification) => {
	const channelProvider = useChannelProvider();

	return channelProvider.getBackendChannel<T>(notificationType);	
}

export default useBackendChannel;