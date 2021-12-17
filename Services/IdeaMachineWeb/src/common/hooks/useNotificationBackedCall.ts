// Framework
import * as React from "react";

// Functionality
import useChannel from "./useChannel";
import useCancellable from "./useCancellable";

// Types
import { NotificationType } from "../modules/channel/types";

export const useNotificationBackedCall = <TNotificationType>(
	notification: NotificationType,
	selector: (notification: TNotificationType) => boolean = null,
	timeout: number = 10000
): [boolean, (cb: () => Promise<void>) => Promise<void>] => {
	const [isRunning, setIsRunning] = React.useState(false);

	const [run, cancel] = useCancellable();

	// Prepare the channel
	const channel = useChannel<TNotificationType>(notification);

	const call = async (asyncCall: () => Promise<void>) => {
		setIsRunning(true);

		// Run the call
		try {
			await asyncCall();
		} catch (e) {
			setIsRunning(false);
			throw e;
		}

		// Wait for the event, and timeout otherwise
		const timeoutHandle = window.setTimeout(cancel, timeout);
		try {
			await run(
				new Promise((resolve) => {
					channel.register(async (notification) => {
						if (selector?.(notification) ?? true) {
							resolve();
						}
					});
				})
			);
		} catch {
		} finally {
			channel.unregister;
			window.clearTimeout(timeoutHandle);
			setIsRunning(false);
		}
	};

	return [isRunning, call];
};

export default useNotificationBackedCall;
