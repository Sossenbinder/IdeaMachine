import { range } from "./arrayUtils";

export const delay = async (seconds: number) => {
	let resolver: ((value: void) => void);
	const promise = new Promise<void>(res => resolver = res);

	window.setTimeout(() => resolver(null), seconds);

	return promise;
}

export const tickCountDownAwaitable = (tickCount: number, msBetweenTicks: number, cb: () => Promise<void>) => {
	return new Promise<void>(resolve => {
		let counter = 0;
		const counterId = window.setInterval(() => {
			cb();
			counter++;

			if (counter === tickCount) {
				clearTimeout(counterId);
				resolve();
			}
		}, msBetweenTicks);
	});
}

export const asyncForEachParallel = async <T>(source: Array<T>, action: (item: T) => Promise<void>, parallelismDegree: number = 25) => {

	const asyncExecutors = range(parallelismDegree)
		.map(() => parallelExecutor(source, action));

	await Promise.all(asyncExecutors);
}

const parallelExecutor = async <T>(source: Array<T>, action: (item: T) => Promise<void>) => {
	while (source.length > 0) {
		const workItem = source.shift();

		if (workItem === undefined) {
			return;
		}

		await action(workItem);
	}
}