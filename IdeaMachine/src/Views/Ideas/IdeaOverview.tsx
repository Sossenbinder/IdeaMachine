// Framework
import * as React from "react";

// Components
import Flex from "common/Components/Flex";
import IdeaInput from "Modules/Ideas/Components/IdeaInput";
import IdeaList from "Modules/Ideas/Components/IdeaList";

export const IdeaOverview: React.FC = () => {
	return (
		<Flex
			direction="Column">
			<IdeaInput />
			<IdeaList />
		</Flex>
	);
}

export default IdeaOverview;