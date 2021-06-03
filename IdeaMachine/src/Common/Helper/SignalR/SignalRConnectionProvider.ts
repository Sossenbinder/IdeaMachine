// Framework
import * as signalR from "@microsoft/signalr";

// Functionality
import ISignalRConnectionProvider from "./Interface/ISignalRConnectionProvider";
import { Notification } from "./types";

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

	public registerNotificationHandler<T>(notificationType: string, handler: (notification: Notification<T>) => Promise<void>): void {
		this.SignalRConnection.on(notificationType.toString(), handler);
	}
}

export default SignalRConnectionProvider;