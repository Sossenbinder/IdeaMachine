// Framework
import * as signalR from "@microsoft/signalr";

// Types
import { Notification } from "./../types";

export interface ISignalRConnectionProvider {
	start(): Promise<void>;

	registerNotificationHandler<T>(notificationType: string, handler: (notification: Notification<T>) => Promise<void>): void;

	readonly SignalRConnection: signalR.HubConnection;
}

export default ISignalRConnectionProvider;