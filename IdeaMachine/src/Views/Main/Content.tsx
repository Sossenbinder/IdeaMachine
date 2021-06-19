// Framework
import * as React from "react";
import { Route } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";
import IdeaInput from "Modules/Ideas/Components/IdeaInput";
import IdeaList from "Modules/Ideas/Components/IdeaList";
import CreateButton from 'Modules/Ideas/Components/CreateButton';
import LogonView from "views/Login/LogonView";
import EmailVerification from "views/EmailVerification/EmailVerification";

// Styles
import styles from "./Styles/Content.module.less";

export const Content: React.FC = () => {
	return (
		<Flex
			className={styles.ContentWrapper}
			space="Around">
			<div className={styles.Content}>
				<Route
					path="/ideainput">
					<IdeaInput />
				</Route>
				<Route
					path="/Logon">
					<LogonView />
				</Route>
				<Route
					path="/VerifyEmail">
					<EmailVerification />
				</Route>
				<Route
					exact={true}
					path="/">
					<IdeaList />
				</Route>
			</div>
			<CreateButton />
		</Flex>
	);
}

export default Content;