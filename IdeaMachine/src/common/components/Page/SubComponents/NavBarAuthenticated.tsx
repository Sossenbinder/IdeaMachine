// Framework
import * as React from "react";
import { Link } from "react-router-dom";

// Components
import MaterialIcon from "common/components/MaterialIcon";
import { Flex } from "common/components";

// Functionality
import useServices from "common/hooks/useServices";

// Types
import { Account } from "modules/account/types";

// Styles
import styles from "./styles/NavBarAuthenticated.module.less";

type Props = {
	account: Account
}

export const NavBarAuthenticated: React.FC<Props> = ({ account }) => {

	const { AccountService } = useServices();

	const onLogoutClick = async () => {
		await AccountService.logout();
	}

	return (
		<Flex
			className={styles.Container}
			crossAlign="Center"
			direction="Row">
			<Link
				to="/account/Overview"
				className={styles.UserName}>
				{account.userName}
			</Link>
			<MaterialIcon
				color="white"
				onClick={onLogoutClick}
				className={styles.Logout}
				iconName="logout"
				size={40} />
		</Flex>
	);
}

export default NavBarAuthenticated;