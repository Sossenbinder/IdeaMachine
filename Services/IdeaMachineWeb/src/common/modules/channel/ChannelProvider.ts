// Functionality
import { IChannel, Channel } from "./Channel";
import ISignalRConnectionProvider from "../../helper/signalR/interface/ISignalRConnectionProvider";

// Types
import BackendNotification from "../../helper/signalR/Notifications";
import { Notification } from "../../helper/signalR/types";
import { NotificationType } from "./types";

export interface IChannelProvider {
	getChannel<TPayload>(notificationType: NotificationType): IChannel<TPayload>;
	getBackendChannel<TPayload>(notificationType: BackendNotification): IChannel<Notification<TPayload>>;
}

export class ChannelProvider implements IChannelProvider {
	private _channels: Map<NotificationType, IChannel<any>>;

	private _signalRContextProvider: ISignalRConnectionProvider;

	constructor(signalRContextProvider: ISignalRConnectionProvider) {
		this._signalRContextProvider = signalRContextProvider;
		this._channels = new Map<NotificationType, IChannel<any>>();
	}

	getChannel<TPayload>(notificationType: NotificationType): IChannel<TPayload> {
		const channelKnown = this._channels.has(notificationType);

		if (!channelKnown) {
			const channel = new Channel<TPayload>();
			this._channels.set(notificationType, channel);
		}

		const channel = this._channels.get(notificationType);

		return channel;
	}

	getBackendChannel<TPayload>(notificationType: BackendNotification): IChannel<Notification<TPayload>> {
		const stringifiedNotification = BackendNotification[notificationType] as NotificationType;

		const channelKnown = this._channels.has(stringifiedNotification);
		const channel = this.getChannel<Notification<TPayload>>(stringifiedNotification);

		if (!channelKnown) {
			this._signalRContextProvider.register(notificationType, channel.publish);
		}

		return channel;
	}
}
