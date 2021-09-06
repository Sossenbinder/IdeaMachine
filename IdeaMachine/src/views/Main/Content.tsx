// Framework
import * as React from "react";
import { Route } from "react-router-dom";

// Components
import Flex from "common/components/Flex";
import IdeaListAll from "modules/Ideas/Components/IdeaListAll";
import CreateButton from 'modules/Ideas/Components/CreateButton';
import LogonView from "views/Login/LogonView";
import EmailVerification from "views/EmailVerification/EmailVerification";
import AccountRoutes from "views/Account/AccountRoutes";
import IdeaRoutes from "views/Ideas/IdeaRoutes";

// Styles
import styles from "./styles/Content.module.less";

export const Content: React.FC = () => {
	return (
		<Flex
			className={styles.ContentWrapper}
			space="Around">
			<div className={styles.Content}>
				<div className={styles.InnerContentWrapper}>
					<Route
						path="/Logon">
						<LogonView />
					</Route>
					<Route
						path="/VerifyEmail">
						<EmailVerification />
					</Route>
					<Route
						path="/Account">
						<AccountRoutes />
					</Route>
					<Route
						path="/Idea">
						<IdeaRoutes />
					</Route>
					<Route
						exact={true}
						path="/">
						<IdeaListAll />
					</Route>
				</div>
			</div>
			<CreateButton />
		</Flex>
	);
}

export default Content;