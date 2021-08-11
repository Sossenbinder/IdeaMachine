// Framework
import * as React from "react";
import { Route, Switch } from "react-router-dom";

// Components
import IdeaInput from "modules/Ideas/Components/Input/IdeaInput";
import IdeaInfo from "modules/Ideas/Components/Info/IdeaInfo";
import IdeaListOwn from "modules/Ideas/Components/IdeaListOwn";

export const IdeaRoutes: React.FC = () => {
	return (
		<Switch>
			<Route
				path="/idea/own">
				<IdeaListOwn />
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