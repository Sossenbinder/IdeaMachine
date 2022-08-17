// Functionality
import { createSingleReducer } from "common/redux/reducer/CrudReducer";

// Types
import { Account } from "modules/account/types";

export const reducer = createSingleReducer<Account>({
	actionIdentifier: "Account",
	initialState: {
		isAnonymous: true,
	} as Account,
});

export default reducer;
