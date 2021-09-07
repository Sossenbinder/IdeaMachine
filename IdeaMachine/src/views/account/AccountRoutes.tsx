// Framework
import * as React from "react";
import { Route } from "react-router-dom";

// Components
import AccountOverview from "modules/account/components/account/AccountOverview";

export const AccountRoutes: React.FC = () => {
	return (
		<>
			<Route path="/account/overview">
				<AccountOverview />
			</Route>
		</>
	);
}

export default AccountRoutes;