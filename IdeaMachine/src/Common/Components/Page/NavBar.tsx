// Framework
import * as React from "react";
import { Button } from "@material-ui/core";
import { withRouter, RouteComponentProps } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";

// Functionality

// Types

// Styles
import styles from "./Styles/NavBar.module.less";

type Props = RouteComponentProps;

export const NavBar: React.FC<Props> = ({ history }) => {
	return (
		<Flex
			className={styles.NavBar}
			space="Between">
			<h2
				className={styles.Label}
				onClick={() => history.push("/")}>
				IdeaMachine
			</h2>
			<Flex
				className={styles.LoginSection}
				direction="Row"
				crossAlignSelf="Center">
				<Button
					color="primary"
					onClick={() => history.push("/Logon/Login")}
					variant="contained">
					Login
				</Button>
				<Button
					onClick={() => history.push("/Logon/Register")}
					variant="contained">
					Register
				</Button>
			</Flex>
		</Flex>
	);
}

export default withRouter(NavBar);