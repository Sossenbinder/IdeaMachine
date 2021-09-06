// Framework
import * as React from "react";
import { Route } from "react-router-dom";

// Components
import AccountOverview from "modules/Account/Components/Account/AccountOverview";

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