// Framework
import { useContext } from "react";

// Components
import { AccountContext } from "modules/account/helper/accountContext";

export const useAccount = () => useContext(AccountContext);

export default useAccount;