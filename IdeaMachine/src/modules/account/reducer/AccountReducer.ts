// Functionality
import { createSingleReducer } from "common/redux/reducer/CrudReducer";

// Types
import { Account } from "modules/account/types";

export const reducer = createSingleReducer<Account>({
	actionIdentifier: "Account",
});

export default reducer;