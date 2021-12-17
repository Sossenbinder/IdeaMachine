// Functionality
import Cancellable from "common/helper/promise";

export const useCancellable = () => {
	const cancelHandles: Array<(reason: any) => void> = [];

	const cancel = () => {
		for (const handle of cancelHandles) {
			handle({});
		}
	};

	const run = async (promise: Promise<void>): Promise<void> => {
		const cancellable = new Cancellable((resolve, reject) => {
			cancelHandles.push(reject);

			promise.then(resolve, reject);
		});

		return cancellable.Promise;
	};

	return [run, cancel];
};

export default useCancellable;
