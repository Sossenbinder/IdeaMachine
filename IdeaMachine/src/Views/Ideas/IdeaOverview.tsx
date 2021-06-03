// Framework
import * as React from "react";

// Components
import Flex from "common/Components/Flex";
import IdeaInput from "Modules/Ideas/Components/IdeaInput";
import IdeaList from "Modules/Ideas/Components/IdeaList";

// Functionality

// Types

// Styles
import styles from "./Styles/IdeaOverview.module.less";

type Props = {

}

export const IdeaOverview: React.FC<Props> = () => {
	return (
		<Flex
			direction="Column">
			<IdeaInput />
			<IdeaList />
		</Flex>
	);
}

export default IdeaOverview;