// Framework
import * as signalR from "@microsoft/signalr";

// Types
import { Notification } from "common/helper/signalR/types";
import NotificationType from "../Notifications";

export interface ISignalRConnectionProvider {
	start(): Promise<void>;

	on<T>(notificationType: NotificationType, handler: (notification: Notification<T>) => Promise<void>): void;

	off<T>(notificationType: NotificationType, handler: (notification: Notification<T>) => Promise<void>): void;

	readonly SignalRConnection: signalR.HubConnection;
}

export default ISignalRConnectionProvider;