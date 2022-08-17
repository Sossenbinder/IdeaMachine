// Framework
import { useContext } from "react";

// Components
import { AccountContext } from "modules/account/helper/accountContext";

export const useAccount = () => {
	const context = useContext(AccountContext);
	return context.account;
};

export default useAccount;
