// Types
import BackendNotification from "common/helper/signalR/Notifications";

export type FunctionHandler<T> = (input: T) => Promise<void>;

export type NotificationType = BackendNotification;