// Framework
import * as React from "react";
import { useSelector } from "react-redux";

// Types
import { Account } from "../types";
import { ReduxStore } from "../../../common/redux/store";

export const AccountContext = React.createContext({} as Account);

export const AccountContextProvider = ({ children }: { children: React.ReactNode }) => {
	const account = useSelector<ReduxStore, Account>((store) => store.accountReducer.data);

	return <AccountContext.Provider value={account}>{children}</AccountContext.Provider>;
};

export default AccountContextProvider;
