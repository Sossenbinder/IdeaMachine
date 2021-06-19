// Framework
import * as React from "react";

// Components
import MaterialIcon from "common/Components/MaterialIcon";
import { Flex } from "common/Components";

// Functionality
import useServices from "common/Hooks/useServices";

// Types
import { Account } from "modules/Account/types";

// Styles
import styles from "./Styles/NavBarAuthenticated.module.less";

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
			<span
				className={styles.UserName}>
				{account.userName}
			</span>
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