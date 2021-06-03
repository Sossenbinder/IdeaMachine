// Framework
import * as React from "react";
import { withRouter, RouteComponentProps } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";

// Functionality

// Types

// Styles
import styles from "./Styles/NavBar.module.less";

type Props = RouteComponentProps;

export const NavBar: React.FC<Props> = ({ history }) => {

	const onHomeRoute = () => history.push("/");

	return (
		<Flex className={styles.NavBar}>
			<h2
				className={styles.Label}
				onClick={onHomeRoute}>
				IdeaMachine
			</h2>
		</Flex>
	);
}

export default withRouter(NavBar);