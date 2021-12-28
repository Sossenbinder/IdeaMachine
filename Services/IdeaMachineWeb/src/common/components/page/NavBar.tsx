// Framework
import * as React from "react";
import { connect } from "react-redux";
import { withRouter, RouteComponentProps } from "react-router-dom";

// Components
import Flex from "common/components/Flex";
import LinkButton from "common/components/controls/LinkButton";
import NavBarAnon from "./subComponents/NavBarAnon";
import NavBarAuthenticated from "./subComponents/NavBarAuthenticated";
import NotificationBell from "./notifications/NotificationBell";
import Separator from "common/components/controls/Separator";

// Types
import { Account } from "modules/account/types";
import { ReduxStore } from "common/redux/store";

// Styles
import styles from "./styles/NavBar.module.scss";

type ReduxProps = {
	account: Account;
};

type Props = RouteComponentProps & ReduxProps;

export const NavBar: React.FC<Props> = ({ account, history }) => {
	return (
		<Flex className={styles.NavBar} space="Between">
			<Flex direction="Row" crossAlign="Center" className={styles.Branding} onClick={() => history.push("/")}>
				<img className={styles.BrandIcon} src="/Resources/Icons/ideablub.svg" />
				<h2>IdeaMachine</h2>
			</Flex>
			<Flex className={styles.ActionSection} direction="Row" crossAlignSelf="Center" mainAlign="End">
				<LinkButton to="/idea/own" color="primary">
					My ideas
				</LinkButton>
				<NotificationBell />
				<Separator direction="Vertical" width="20px" />
				<Choose>
					<When condition={!account || account.isAnonymous}>
						<NavBarAnon />
					</When>
					<Otherwise>
						<NavBarAuthenticated account={account} />
					</Otherwise>
				</Choose>
			</Flex>
		</Flex>
	);
};

const mapStateToProps = (state: ReduxStore): ReduxProps => {
	return {
		account: state.accountReducer.data
	};
};

export default connect(mapStateToProps)(withRouter(NavBar));
