// Framework
import * as React from "react";

// Components
import Flex from "common/Components/Flex";
import SignInDialog from "modules/Account/Components/Login/SignInDialog";

// Functionality

// Types

// Styles
import styles from "./Styles/LogonView.module.less";

type Props = {

}

export const LogonView: React.FC<Props> = () => {
	return (
		<Flex
			direction="Column">
			<SignInDialog />
		</Flex>
	);
}

export default LogonView;