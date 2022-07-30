// Functionality
import { removeAt } from "./arrayUtils";

type ListenerInfo<T> = {
	listenerId: number;
	listener: (args: T) => Promise<void>;
};

export default class AsyncEvent<T> {
	private _registeredListeners: Array<ListenerInfo<T>>;

	private _listenerCounter = 1;

	constructor() {
		this._registeredListeners = [];
	}

	public register(func: (args: T) => Promise<void>): number {
		const listenerId = this._listenerCounter;

		this._listenerCounter++;

		this._registeredListeners.push({
			listenerId,
			listener: func,
		});

		return listenerId;
	}

	public unregister(listenerId: number): void {
		const respectiveListenerInfoIndex = this._registeredListeners.findIndex((x) => x.listenerId === listenerId);

		removeAt(this._registeredListeners, respectiveListenerInfoIndex);
	}

	public async raise(args: T): Promise<void> {
		await Promise.all(this._registeredListeners.map((registeredListener) => registeredListener.listener(args)));
	}
}
