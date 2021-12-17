export type PromiseSignature = (resolve: (value: void | object | PromiseLike<object>) => void, reject: (reason?: any) => void) => void;

export default class Cancellable {
	private _promise: Promise<void>;
	get Promise(): Promise<void> {
		return this._promise;
	}

	private _cancelHandle: (reason?: any) => void;

	constructor(signature: PromiseSignature) {
		this._promise = new Promise((resolve, reject) => {
			this._cancelHandle = reject;
			signature(resolve, reject);
		});
	}

	public cancel = () => this._cancelHandle();
}
