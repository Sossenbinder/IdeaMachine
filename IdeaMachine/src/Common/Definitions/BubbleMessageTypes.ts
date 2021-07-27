export enum MessageType {
	Info,
	Warning,
	Error,
}

export type BubbleMessage = {
	message: string;
	type: keyof MessageType;
	timeStamp: Date;
	timeout?: number;
}