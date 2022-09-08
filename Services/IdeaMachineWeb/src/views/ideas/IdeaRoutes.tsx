// Framework
import * as React from "react";
import { Route, Switch } from "react-router-dom";

// Components
import IdeaInput from "modules/ideas/components/input/IdeaInput";
import IdeaListOwn from "modules/ideas/components/IdeaListOwn";
import IdeaReply from "modules/ideas/components/info/reply/IdeaReply";

const IdeaInfo = React.lazy(() => import("modules/ideas/components/info/IdeaInfo"));
const SuspendedIdeaInfo = () => (
	<React.Suspense fallback={<></>}>
		<IdeaInfo />
	</React.Suspense>
);

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
				<IdeaReply />
			</Route>
			<Route path="/idea/:id" component={SuspendedIdeaInfo} />
		</Switch>
	);
};

export default IdeaRoutes;
