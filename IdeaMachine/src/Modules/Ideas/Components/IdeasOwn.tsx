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

export const IdeasOwn: React.FC<Props> = ({ ideas }) => {

	const { IdeaService } = useServices();

	const ideasRendered = React.useMemo(() => ideas
		.sort((x, y) => y.creationDate.getTime() - x.creationDate.getTime())
		.map(idea => (
			<IdeaListEntry
				idea={idea}
				key={idea.id} />
		)), [ideas]);

	React.useEffect(() => {
		IdeaService.initializeOwnIdeas();
	}, []);

	return (
		<Flex
			className={styles.IdeaList}
			direction="Column">
			{ideasRendered}
		</Flex>
	);
}

const mapStateToProps = (state: ReduxStore): Props => {

	const accountId = state.accountReducer.data.userId;

	return {
		ideas: state.ideaReducer.data.filter(x => x.creatorId === accountId),
	}
};

export default connect(mapStateToProps)(IdeasOwn);