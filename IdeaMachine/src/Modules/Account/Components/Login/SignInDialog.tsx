// Framework
import * as React from "react";
import { Route, withRouter } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";
import SignIn from "./SignIn";
import Register from "./Register";

// Functionality

// Types

// Styles
import styles from "./Styles/SignInDialog.module.less";

export const SignInDialog: React.FC = () => {
	return (
		<Flex
			className={styles.SignInDialogContainer}
			crossAlign="Center"
			direction="Column">
			<Flex
				className={styles.SignInDialogFormContainer}
				direction="Column"
				crossAlign="Center">
				<Route
					exact
					path="/Logon/Login">
					<SignIn />
				</Route>
				<Route
					exact
					path="/Logon/Register">
					<Register />
				</Route>
			</Flex>
		</Flex>
	);
}

export default withRouter(SignInDialog);