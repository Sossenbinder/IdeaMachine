// Functionality
import { createReducer } from "common/redux/reducer/CrudReducer";

// Types
import { Notification } from "common/definitions/NotificationTypes";

export const reducer = createReducer<Notification>({
	actionIdentifier: "Notifications",
	key: "id",
});

export default reducer;