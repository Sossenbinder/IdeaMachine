// Framework
import * as React from "react";

export const useAsyncCall = <T = void>(): [boolean, (cb: () => Promise<T>) => Promise<T>] => {

	const [isRunning, setIsRunning] = React.useState(false);

	const callAsync = async (cb: () => Promise<T>) => {
		setIsRunning(true);

		try {
			return await cb();
		} finally {
			setIsRunning(false);
		}
	};

	return [isRunning, callAsync];
};

export default useAsyncCall;