// Functionality
import { FunctionHandler } from "./types";

export interface IChannel<T> {
	publish(param: T): Promise<void>;
	register(handler: FunctionHandler<T>): void;
	unregister(handler: FunctionHandler<T>): void;
}

export class Channel<T> implements IChannel<T> {

	private _handlers: Array<FunctionHandler<T>>;

	constructor() {
		this._handlers = [];
	}

	public publish = async (param: T): Promise<void> => {
		const handlerFuncs = this._handlers.map(x => x(param));
		await Promise.all(handlerFuncs);
	}

	public register = (handler: FunctionHandler<T>) => {
		this._handlers.push(handler);
	}

	public unregister = (handler: FunctionHandler<T>) => {
		const index = this._handlers.findIndex(x => x === handler);

		if (index !== -1) {
			this._handlers.splice(index, 1);
		}
	}
}