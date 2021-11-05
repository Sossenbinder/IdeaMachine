// Framework
import * as React from "react";
import { useSelector } from "react-redux";

// Types
import { ReduxStore } from "common/redux/store";

// Styles
import MaterialIcon from "common/components/MaterialIcon";
import { Account } from "modules/account/types";

type Props = {
	
}

export const NotificationBell = ({ }: Props) => {

	const notifications = useSelector<ReduxStore, Account>(x => x.accountReducer.data);

	return (
		<MaterialIcon
			size={37.5}
			color="white"
			iconName="notifications" />
	);
}

export default NotificationBell;