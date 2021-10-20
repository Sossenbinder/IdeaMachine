// Functionality
import store from "common/redux/store";
import { reducer as pushNotificationReducer } from "common/redux/reducer/PushNotificationReducer";

// Types
import { PushNotification } from "common/definitions/PushNotificationTypes";

export const addPushMessage = (pushNotification: PushNotification) => {
	store.dispatch(pushNotificationReducer.add(pushNotification));
}