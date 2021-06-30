// Framework
import * as React from "react";
import { Route, Switch } from "react-router-dom";

// Components
import IdeaInput from "modules/Ideas/Components/IdeaInput";
import IdeaInfo from "modules/Ideas/Components/IdeaInfo";
import IdeasOwn from "modules/Ideas/Components/IdeasOwn";

export const IdeaRoutes: React.FC = () => {
	return (
		<Switch>
			<Route
				path="/idea/own">
				<IdeasOwn />
			</Route>
			<Route
				path="/idea/input">
				<IdeaInput />
			</Route>
			<Route
				path="/idea/:id/reply">
				<IdeaInput />
			</Route>
			<Route
				path="/idea/:id"
				render={props => <IdeaInfo {...props} />}
			/>
		</Switch>
	);
}

export default IdeaRoutes;