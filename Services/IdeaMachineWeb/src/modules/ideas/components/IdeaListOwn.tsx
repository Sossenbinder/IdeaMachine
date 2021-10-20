// Framework
import * as React from "react";
import { connect } from "react-redux";

// Components
import IdeaList from "./IdeaList";

// Types
import { ReduxStore } from "common/redux/store";
import { Idea } from "../types";

type ReduxProps = {
	ideas: Array<Idea>;
}

export const IdeaListOwn: React.FC<ReduxProps> = ({ ideas }) => {
	return (
		<IdeaList
			ideas={ideas}
		/>
	);
}

const mapStateToProps = (state: ReduxStore): ReduxProps => {

	const accountId = state.accountReducer.data.userId;

	return {
		ideas: state.ideaReducer.data.filter(x => x.creatorId === accountId),
	}
};

export default connect(mapStateToProps)(IdeaListOwn);