// Framework
import { Action } from "redux";

// Functionality
import { store, ReduxStore } from "common/redux/store";

// Types
import { IModuleService } from "./types";
import { IChannelProvider } from "../channel/ChannelProvider";

export default abstract class ModuleService implements IModuleService {

	public abstract start(): Promise<void>;

	private _store: ReduxStore;

	private _channelProvider: IChannelProvider;

	protected get ChannelProvider(){ 
		return this._channelProvider; 
	}

	protected constructor(channelProvider: IChannelProvider) {
		this._store = store;
		this._channelProvider = channelProvider;
	}

	protected getStore(): ReduxStore {
		return this._store.getState();
	}

	protected dispatch(dispatchAction: Action) {
		this._store.dispatch(dispatchAction);
	}
}