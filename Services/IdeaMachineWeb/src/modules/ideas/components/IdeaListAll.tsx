// Framework
import * as React from "react";

// Components
import IdeaList from "./IdeaList";

// Types
import { ReduxStore } from "common/redux/store";
import { useSelector } from "react-redux";

export const IdeaListAll = () => {
	const ideas = useSelector((state: ReduxStore) => state.ideaReducer.data);

	return <IdeaList ideas={ideas} />;
};

export default IdeaListAll;
