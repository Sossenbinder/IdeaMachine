// Framework
import * as React from "react";
import { connect } from "react-redux";

// Types
import { Account } from "../types";
import { ReduxStore } from "common/redux/store";

export const AccountContext = React.createContext({} as Account);

type ReduxProps = {
	account: Account;
}

export const AccountContextProvider: React.FC<ReduxProps> = ({ account, children }) => (
	<AccountContext.Provider value={account}>
		{ children }
	</AccountContext.Provider>
);

const mapStateToProps = (state: ReduxStore): ReduxProps => {
	return {
		account: state.accountReducer.data,
	};
};

export default connect(mapStateToProps)(AccountContextProvider);