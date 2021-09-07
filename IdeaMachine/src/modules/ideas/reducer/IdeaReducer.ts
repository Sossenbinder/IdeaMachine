// Functionality
import { createReducer } from "common/redux/reducer/CrudReducer";

// Types
import { Idea } from "modules/Ideas/types";

export const reducer = createReducer<Idea>({
	actionIdentifier: "Idea",
	key: "id",
});

export default reducer;