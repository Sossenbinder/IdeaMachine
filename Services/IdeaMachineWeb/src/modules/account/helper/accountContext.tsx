// Framework
import { useMsal } from "@azure/msal-react";
import * as React from "react";

// Types
import { Account } from "../types";

export const AccountContext = React.createContext({} as Account);

export const AccountContextProvider = ({ children }: { children: React.ReactNode }) => {
	const {
		accounts: [account],
	} = useMsal();

	console.log(account);

	return <AccountContext.Provider value={{} as Account}>{children}</AccountContext.Provider>;
};

export default AccountContextProvider;
