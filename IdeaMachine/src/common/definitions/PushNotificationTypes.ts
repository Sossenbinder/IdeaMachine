export enum MessageType {
	Info,
	Warning,
	Error,
}

export type PushNotification = {
	message: string;
	type: keyof typeof MessageType;
	timeStamp: Date;
	timeout?: number;
}