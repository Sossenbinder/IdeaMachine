// Framework
import * as React from "react";
import { connect } from "react-redux";
import { Route, RouteProps } from 'react-router';

// Types
import { Account } from "modules/account/types";
import { ReduxStore } from "common/redux/store";

type ReduxProps = {
	account: Account;
}

export const AuthRoute: React.FC<RouteProps & ReduxProps> = (props) => {

	if (!props.account || props.account.isAnonymous) {
		return <span>Sorry, this route is only for authenticated users</span>;
	}

	return (
		<Route {...props}>
			{props.children}
		</Route>
	);
}

const mapStateToProps = (store: ReduxStore): ReduxProps => ({
	account: store.accountReducer.data,
});

export default connect(mapStateToProps)(AuthRoute);