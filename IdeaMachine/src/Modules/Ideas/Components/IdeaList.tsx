// Framework
import * as React from "react";
import { connect } from "react-redux";

// Components
import Flex from "common/Components/Flex";
import IdeaListEntry from "./IdeaListEntry";

// Functionality
import useServices from "common/Hooks/useServices";

// Types
import { ReduxStore } from "common/Redux/store";
import { Idea } from "modules/Ideas/types";

// Styles
import styles from "./Styles/IdeaList.module.less";

type Props = {
	ideas: Array<Idea>;
}

export const IdeaList: React.FC<Props> = ({ ideas }) => {

	const { IdeaService } = useServices();

	const ideasRendered = React.useMemo(() => ideas.map(idea => (
		<IdeaListEntry
			idea={idea}
			key={idea.id} />
	)), [ideas]);

	React.useEffect(() => {
		IdeaService.initializeIdeas();
	}, []);

	return (
		<Flex
			className={styles.IdeaList}
			direction="Column">
			{ideasRendered}
		</Flex>
	);
}

const mapStateToProps = (state: ReduxStore): Props => ({
	ideas: state.ideaReducer.data,
});

export default connect(mapStateToProps)(IdeaList);