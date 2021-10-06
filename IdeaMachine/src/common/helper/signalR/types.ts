export enum Operation {
	Create,
	Update,
	Delete
}

export type BackendNotification<T> = {

	readonly operation: Operation;

	readonly notificationType: string;

	readonly payload: T;
}

export type Notification<T> = Omit<BackendNotification<T>, "NotificationType">;