// Framework
import * as React from "react";

// Types
import { Account } from "../types";

export type AccountContext = {
	account: Account;
	setAccount: (account: Account) => void;
};

export const AccountContext = React.createContext<AccountContext>({
	account: undefined,
	setAccount: () => void 0,
});

export const AccountContextProvider = ({ children }: { children: React.ReactNode }) => {
	const [ideaMachineAccount, setIdeaMachineAccount] = React.useState<Account>({
		isAnonymous: true,
	} as Account);

	return (
		<AccountContext.Provider
			value={{
				account: ideaMachineAccount,
				setAccount: setIdeaMachineAccount,
			}}
		>
			{children}
		</AccountContext.Provider>
	);
};

export default AccountContextProvider;
