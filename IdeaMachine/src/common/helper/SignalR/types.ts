
export enum Operation {
	Create,
	Update,
	Delete
}

export type Notification<T> = {

	readonly operation: Operation;

	readonly notificationType: string;

	readonly payload: T;
}