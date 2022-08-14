// Framework
import { useMsal } from "@azure/msal-react";
import * as React from "react";

// Types
import { Account } from "../types";

export const AccountContext = React.createContext({} as Account);

export const AccountContextProvider = ({ children }: { children: React.ReactNode }) => {
	const [ideaMachineAccount, setIdeaMachineAccount] = React.useState<Account>();

	const {
		accounts: [account],
	} = useMsal();

	React.useEffect(() => {
		const claims = account.idTokenClaims;

		setIdeaMachineAccount({
			userName: claims["given_name"] as string,
			isAnonymous: false,
			userId: account.nativeAccountId,
			email: claims["emails"][0] as string,
		});
	}, [account]);

	return <AccountContext.Provider value={ideaMachineAccount}>{children}</AccountContext.Provider>;
};

export default AccountContextProvider;
