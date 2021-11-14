// Framework
import * as signalR from "@microsoft/signalr";

// Functionality
import ISignalRConnectionProvider from "./interface/ISignalRConnectionProvider";

// Types
import { Notification } from "./types";
import BackendNotification from "./Notifications";

export class SignalRConnectionProvider implements ISignalRConnectionProvider {

	public readonly SignalRConnection: signalR.HubConnection;

	public constructor() {
		this.SignalRConnection = new signalR.HubConnectionBuilder()
			.withUrl("/signalRHub")
			.configureLogging(signalR.LogLevel.Error)
			.build();
	}

	async start(): Promise<void> {
		await this.SignalRConnection.start();
	}

	public register<T>(notificationType: BackendNotification, handler: (notification: Notification<T>) => Promise<void>): void {
		this.SignalRConnection.on(BackendNotification[notificationType], handler);
	}

	public unregister<T>(notificationType: BackendNotification, handler: (notification: Notification<T>) => Promise<void>): void {
		this.SignalRConnection.off(BackendNotification[notificationType], handler);
	}
}

export default SignalRConnectionProvider;