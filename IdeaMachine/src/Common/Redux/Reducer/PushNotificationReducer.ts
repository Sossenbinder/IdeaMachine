// Functionality
import { createReducer } from "common/redux/Reducer/CrudReducer";

// Types
import { PushNotification } from "common/definitions/PushNotificationTypes";

export const reducer = createReducer<PushNotification>({
	actionIdentifier: "BubbleMessage",
	key: "timeStamp",
});

export default reducer;