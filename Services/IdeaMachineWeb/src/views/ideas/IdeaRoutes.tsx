// Framework
import * as React from "react";
import { Route, Switch } from "react-router-dom";

// Components
import IdeaInput from "modules/ideas/components/input/IdeaInput";
import IdeaInfo from "modules/ideas/components/info/IdeaInfo";
import IdeaListOwn from "modules/ideas/components/IdeaListOwn";

export const IdeaRoutes: React.FC = () => {
	return (
		<Switch>
			<Route path="/idea/own">
				<IdeaListOwn />
			</Route>
			<Route path="/idea/input">
				<IdeaInput />
			</Route>
			<Route path="/idea/:id/reply">
				<IdeaInput />
			</Route>
			<Route path="/idea/:id" component={IdeaInfo} />
		</Switch>
	);
};

export default IdeaRoutes;
