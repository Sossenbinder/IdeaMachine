// Framework
import * as React from "react";
import { connect } from "react-redux";
import { withRouter, RouteComponentProps, Link } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";
import NavBarAnon from "./SubComponents/NavBarAnon";
import NavBarAuthenticated from "./SubComponents/NavBarAuthenticated";

// Types
import { Account } from "modules/Account/types";
import { ReduxStore } from "common/Redux/store";

// Styles
import styles from "./Styles/NavBar.module.less";

type ReduxProps = {
	account: Account;
}

type Props = RouteComponentProps & ReduxProps;

export const NavBar: React.FC<Props> = ({ account, history }) => {
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
				<Link
					to="/idea/own">
					My ideas
				</Link>
				<Choose>
					<When condition={!account || account.isAnonymous}>
						<NavBarAnon />
					</When>
					<Otherwise>
						<NavBarAuthenticated
							account={account} />
					</Otherwise>
				</Choose>
			</Flex>
		</Flex>
	);
}

const mapStateToProps = (state: ReduxStore): ReduxProps => {
	return {
		account: state.accountReducer.data,
	};
}

export default connect(mapStateToProps)(withRouter(NavBar));