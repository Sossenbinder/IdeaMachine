import { range } from "./arrayUtils";

export const delay = async (seconds: number) => {
	let resolver: (value: void) => void;
	const promise = new Promise<void>((res) => (resolver = res));

	window.setTimeout(() => resolver(null), seconds);

	return promise;
};

export const tickCountDownAwaitable = (tickCount: number, msBetweenTicks: number, cb: () => Promise<void>) => {
	return new Promise<void>((resolve) => {
		let counter = 0;
		const counterId = window.setInterval(() => {
			cb();
			counter++;

			if (counter >= tickCount) {
				clearTimeout(counterId);
				resolve();
			}
		}, msBetweenTicks);
	});
};

export const asyncForEachParallel = async <T>(source: Array<T>, action: (item: T) => Promise<void>, parallelismDegree = 25) => {
	const asyncExecutors = range(parallelismDegree).map(() => parallelExecutor(source, action));

	await Promise.all(asyncExecutors);
};

const parallelExecutor = async <T>(source: Array<T>, action: (item: T) => Promise<void>) => {
	while (source.length > 0) {
		const workItem = source.shift();

		if (workItem === undefined) {
			return;
		}

		await action(workItem);
	}
};

// @ts-ignore
async function* mergeAsyncIterators<TOut>(iterators: Array<AsyncGenerator<TOut, void>>) {
	const getIndexedNextPromise = (iterator: AsyncGenerator<TOut, void>, index: number) =>
		iterator.next().then((iteratorResult) => ({
			index,
			iteratorResult,
		}));

	type IteratorInfo = {
		index: number;
		iteratorResult: IteratorResult<TOut, void>;
	};

	const outstandingPromises: Array<Promise<IteratorInfo>> = iterators.map(getIndexedNextPromise);

	const cleanUpResolvers = new Array<() => void>();

	let outstandingCount = iterators.length;
	try {
		while (outstandingCount !== 0) {
			const { index, iteratorResult } = await Promise.race(outstandingPromises);

			if (iteratorResult.done) {
				// This promise will never resolve, so Promise.race will ignore it
				outstandingPromises[index] = new Promise<IteratorInfo>((res) =>
					cleanUpResolvers.push(() =>
						res({
							index: 0,
							iteratorResult: {
								value: void 0,
								done: true,
							},
						}),
					),
				);
				outstandingCount--;
			} else {
				outstandingPromises[index] = getIndexedNextPromise(iterators[index], index);
				yield iteratorResult.value;
			}
		}
	} finally {
		// Drain remaining iterators
		for (const iterator of iterators) {
			for await (const item of iterator) {
				yield item;
			}
		}

		cleanUpResolvers.forEach((x) => x());
	}
}
