// Functionality
import { createReducer } from "common/redux/reducer/CrudReducer";

// Types
import { Idea } from "modules/ideas/types";

export const reducer = createReducer<Idea>({
	actionIdentifier: "Idea",
	key: "id",
});

export default reducer;