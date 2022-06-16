// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";

// Styles
//import styles from "./IdeaReply.module.scss";
import Card from "../../Card";

type Props = {};

export const IdeaReply = ({}: Props) => {
	return (
		<Card>
			<Flex direction="Column">Reply</Flex>
		</Card>
	);
};

export default IdeaReply;
