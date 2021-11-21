// Types
import BackendNotification from "common/helper/signalR/Notifications";

export type FunctionHandler<T> = (input: T) => Promise<void>;

export enum Notification {
	ProfilePictureUpdated,
} 

export type NotificationType = BackendNotification | Notification;