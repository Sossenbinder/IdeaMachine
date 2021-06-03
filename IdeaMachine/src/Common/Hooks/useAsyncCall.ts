// Framework
import * as React from "react";

export const useAsyncCall = (): [boolean, (cb: () => Promise<void>) => Promise<void>] => {

	const [isRunning, setIsRunning] = React.useState(false);

	const callAsync = async (cb: () => Promise<void>) => {
		setIsRunning(true);

		try {
			await cb();
		} finally {
			setIsRunning(false);
		}
	};

	return [isRunning, callAsync];
};

export default useAsyncCall;