// Framework
import * as React from "react";
import { Route, withRouter } from "react-router-dom";

// Components
import Flex from "common/components/Flex";
import SignIn from "./SignIn";
import Register from "./Register";
import SocialLoginErrorView from "./SocialLoginErrorView";

// Functionality

// Types

// Styles
import styles from "./styles/SignInDialog.module.scss";

export const SignInDialog: React.FC = () => {
	return (
		<Flex className={styles.SignInDialogFormContainer} direction="Column" crossAlign="Center" crossAlignSelf="Center">
			<Route exact path="/Logon/login">
				<SignIn />
			</Route>
			<Route exact path="/Logon/Register">
				<Register />
			</Route>
			<Route path="/Logon/Error/:errorCode">
				<SocialLoginErrorView />
			</Route>
		</Flex>
	);
};

export default withRouter(SignInDialog);
