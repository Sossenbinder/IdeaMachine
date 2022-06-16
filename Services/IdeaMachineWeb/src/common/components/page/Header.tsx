// Framework
import { Group } from "@mantine/core";
import * as React from "react";

// Framework
import { useSelector } from "react-redux";

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
import { useHistory } from "react-router";

type Props = {};

export const Header = ({}: Props) => {
	const account = useSelector<ReduxStore, Account>((state) => state.accountReducer.data);
	const history = useHistory();

	return (
		<Flex className={styles.NavBar} space="Between">
			<Flex direction="Row" crossAlign="Center" className={styles.Branding} onClick={() => history.push("/")}>
				<img className={styles.BrandIcon} src="/Resources/Icons/ideablub.svg" />
				<span>IdeaMachine</span>
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

export default Header;
