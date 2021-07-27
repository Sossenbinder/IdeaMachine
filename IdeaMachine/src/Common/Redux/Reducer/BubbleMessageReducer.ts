// Functionality
import { createReducer } from "common/redux/Reducer/CrudReducer";

// Types
import { BubbleMessage } from "common/definitions/BubbleMessageTypes";

export const reducer = createReducer<BubbleMessage>({
	actionIdentifier: "BubbleMessage",
	key: "timeStamp",
});

export default reducer;