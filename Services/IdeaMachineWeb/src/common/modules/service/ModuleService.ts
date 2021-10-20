// Framework
import { Action } from "redux";

// Functionality
import { store, ReduxStore } from "common/redux/store";

// Types
import { IModuleService } from "./types";
import ISignalRConnectionProvider from "common/helper/signalR/interface/ISignalRConnectionProvider";
import { Notification } from "common/helper/signalR/types";
import NotificationType from "common/helper/signalR/Notifications";

export default abstract class ModuleService implements IModuleService {

	public abstract start(): Promise<void>;

	private _store: ReduxStore;

	private _signalRConnectionProvider: ISignalRConnectionProvider;

	protected constructor(signalRConnectionProvider: ISignalRConnectionProvider) {
		this._store = store;
		this._signalRConnectionProvider = signalRConnectionProvider;
	}

	protected getStore(): ReduxStore {
		return this._store.getState();
	}

	protected registerForNotification = <T>(notificationType: NotificationType, handler: (notification: Notification<T>) => Promise<void>) => 
		this._signalRConnectionProvider.on(notificationType, handler);

	protected unRegisterForNotification = <T>(notificationType: NotificationType, handler: (notification: Notification<T>) => Promise<void>) => 
		this._signalRConnectionProvider.off(notificationType, handler);

	protected dispatch(dispatchAction: Action) {
		this._store.dispatch(dispatchAction);
	}
}