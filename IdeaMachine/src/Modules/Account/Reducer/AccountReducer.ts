// Functionality
import { createSingleReducer } from "common/Redux/Reducer/CrudReducer";

// Types
import { Account } from "modules/Account/types";

export const reducer = createSingleReducer<Account>({
	actionIdentifier: "Account",
});

export default reducer;