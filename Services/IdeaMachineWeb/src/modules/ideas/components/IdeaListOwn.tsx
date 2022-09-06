// Framework
import * as React from "react";
import { connect, useSelector } from "react-redux";

// Components
import IdeaList from "./IdeaList";

// Types
import { ReduxStore } from "common/redux/store";
import useAccount from "common/hooks/useAccount";

export const IdeaListOwn = () => {
	const account = useAccount();
	const ideas = useSelector((state: ReduxStore) => state.ideaReducer.data.filter((x) => x.creatorId === account.userId));
	return <IdeaList ideas={ideas} />;
};

export default IdeaListOwn;
