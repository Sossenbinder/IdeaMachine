// Framework
import * as React from "react";
import { connect } from "react-redux";
import { withRouter, RouteComponentProps, Link } from "react-router-dom";

// Components
import Flex from "common/components/Flex";
import LinkButton from "common/components/Controls/LinkButton";
import NavBarAnon from "./SubComponents/NavBarAnon";
import NavBarAuthenticated from "./SubComponents/NavBarAuthenticated";
import Separator from "common/components/Controls/Separator";

// Types
import { Account } from "modules/Account/types";
import { ReduxStore } from "common/redux/store";

// Styles
import styles from "./styles/NavBar.module.less";

type ReduxProps = {
	account: Account;
}

type Props = RouteComponentProps & ReduxProps;

export const NavBar: React.FC<Props> = ({ account, history }) => {
	return (
		<Flex
			className={styles.NavBar}
			space="Between">
			<Flex
				direction="Row"
				crossAlign="Center"
				className={styles.Branding}
				onClick={() => history.push("/")}>
				<img className={styles.BrandIcon} src="/Resources/Icons/ideablub.svg" />
				<h2>IdeaMachine</h2>
			</Flex>
			<Flex
				className={styles.LoginSection}
				direction="Row"
				crossAlignSelf="Center">
				<LinkButton
					to="/idea/own"
					color="primary">
					My ideas
				</LinkButton>
				<Separator
					direction="Vertical"
					width="20px" />
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