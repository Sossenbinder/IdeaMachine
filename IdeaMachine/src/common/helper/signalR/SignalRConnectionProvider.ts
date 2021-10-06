// Framework
import * as signalR from "@microsoft/signalr";

// Functionality
import ISignalRConnectionProvider from "./interface/ISignalRConnectionProvider";

// Types
import { Notification } from "./types";
import NotificationType from "./Notifications";

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

	public on<T>(notificationType: NotificationType, handler: (notification: Notification<T>) => Promise<void>): void {
		this.SignalRConnection.on(NotificationType[notificationType], handler);
	}

	public off<T>(notificationType: NotificationType, handler: (notification: Notification<T>) => Promise<void>): void {
		this.SignalRConnection.off(NotificationType[notificationType], handler);
	}
}

export default SignalRConnectionProvider;