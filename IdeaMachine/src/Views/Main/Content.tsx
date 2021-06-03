// Framework
import * as React from "react";
import { Route } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";
import IdeaInput from "Modules/Ideas/Components/IdeaInput";
import IdeaList from "Modules/Ideas/Components/IdeaList";
import CreateButton from 'Modules/Ideas/Components/CreateButton';

// Functionality

// Types

// Styles
import styles from "./Styles/Content.module.less";

type Props = {

}

export const Content: React.FC<Props> = () => {
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