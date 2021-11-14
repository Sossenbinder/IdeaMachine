// Functionality
import useChannelProvider from "./useChannelProvider";

// Types
import { NotificationType } from "../modules/channel/types";

export const useChannel = <T>(notificationType: NotificationType) => {
	const channelProvider = useChannelProvider();

	return channelProvider.getChannel<T>(notificationType);	
}

export default useChannel;