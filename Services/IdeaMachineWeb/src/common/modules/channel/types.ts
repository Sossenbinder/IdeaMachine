// Types
import BackendNotification from "common/helper/signalR/Notifications";

export type FunctionHandler<T> = (input: T) => Promise<void>;

export enum Notification {
	UpdateProfilePictureTriggered
}

export type NotificationType = keyof typeof BackendNotification | keyof typeof Notification;
