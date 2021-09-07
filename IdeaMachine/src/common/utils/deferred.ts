export default class Deferred {

	private _resolve: (value: void | PromiseLike<void>) => void;
	private _reject: (reason?: any) => void;

	private readonly _promise: Promise<void>;

	get Promise(): Promise<void> {
		return this._promise;
	}

	constructor() {
		this._promise = new Promise<void>((resolve, reject) => {
			this._resolve = resolve;
			this._reject = reject;
		})
	}

	public resolve = () => this._resolve();
	public reject = () => this._reject();
}