// Framework
import * as React from "react";
import { Route } from "react-router-dom";

// Components
import Flex from "common/components/Flex";
import IdeaListAll from "modules/ideas/components/IdeaListAll";
import CreateButton from "modules/ideas/components/CreateButton";
import LogonView from "views/login/LogonView";
import EmailVerification from "views/emailVerification/EmailVerification";
import AccountRoutes from "views/account/AccountRoutes";
import IdeaRoutes from "views/ideas/IdeaRoutes";

// Styles
import styles from "./styles/Content.module.scss";

export const Content: React.FC = () => {
	return (
		<Flex className={styles.ContentWrapper} space="Around">
			<div className={styles.Content}>
				<div className={styles.InnerContentWrapper}>
					<Route path="/Logon">
						<LogonView />
					</Route>
					<Route path="/VerifyEmail">
						<EmailVerification />
					</Route>
					<Route path="/account">
						<AccountRoutes />
					</Route>
					<Route path="/Idea">
						<IdeaRoutes />
					</Route>
					<Route exact={true} path="/">
						<IdeaListAll />
					</Route>
				</div>
			</div>
			<CreateButton />
		</Flex>
	);
};

export default Content;
