export type Notification = {
	heading: string;
	message: string;
	action: () => void;
	id: string;
}