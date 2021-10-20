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

export const IdeaListAll: React.FC<ReduxProps> = ({ ideas }) => {
	return (
		<IdeaList
			ideas={ideas}
		/>
	);
}


const mapStateToProps = (state: ReduxStore): ReduxProps => ({
	ideas: state.ideaReducer.data,
});

export default connect(mapStateToProps)(IdeaListAll);