// Framework
import * as signalR from "@microsoft/signalr";

// Types
import { Notification } from "common/helper/signalR/types";
import BackendNotification from "../Notifications";

export interface ISignalRConnectionProvider {
	start(): Promise<void>;

	register<T>(notificationType: BackendNotification, handler: (notification: Notification<T>) => Promise<void>): void;

	unregister<T>(notificationType: BackendNotification, handler: (notification: Notification<T>) => Promise<void>): void;

	readonly SignalRConnection: signalR.HubConnection;
}

export default ISignalRConnectionProvider;