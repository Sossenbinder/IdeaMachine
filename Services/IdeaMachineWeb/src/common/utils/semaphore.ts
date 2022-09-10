import { delay } from "../helper/asyncUtils";

export default class Semaphore {

	private _maxLock: number;

	private readonly _waitResolvers: Array<() => void>;

	constructor(maxLock = 1) {
		this._maxLock = maxLock;
	}

	public enter(): boolean {
		if (this._maxLock === 0) {
			return false;
		}

		this._maxLock--;
		return true;
	}

	public async wait(timeout: number = null): Promise<void> {

		if (this._maxLock > 0) {
			this._maxLock--;
			return Promise.resolve();
		}

		let resolver: () => void;
		const deferred = new Promise<boolean>(resolve => {
			resolver = () => resolve(true);
		});

		this._waitResolvers.push(resolver);

		const promises = [deferred];

		if (timeout !== undefined) {
			promises.push((async () => {
				await delay(timeout);
				return false;
			})());
		}

		const raceResult = await Promise.race(promises);

		if (!raceResult) {
			throw new Error("Timeout occured")
		}

		if (this._maxLock > 0) {
			this._maxLock--;
		}
	}

	public release(): void {
		this._maxLock++;

		this._waitResolvers.shift()();
	}
}