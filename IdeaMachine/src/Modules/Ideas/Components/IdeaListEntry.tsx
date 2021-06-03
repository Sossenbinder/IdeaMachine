// Framework
import * as React from "react";

// Components
import { Grid, Cell, Flex } from "common/Components";

// Functionality
import { getUsDate, getUsTime } from "common/Utils/timeUtils";

// Types
import { Idea } from "../types";

// Styles
import styles from "./Styles/IdeaListEntry.module.less";

type Props = {
	idea: Idea;
}

export const IdeaListEntry: React.FC<Props> = ({ idea: { shortDescription, creationDate, longDescription } }) => {
	return (
		<Grid
			className={styles.Container}
			gridProperties={{
				gridTemplateColumns: "9fr 1fr",
				gridTemplateRows: "9fr 1fr"
			}}>
			<span>{shortDescription}</span>
			<Flex direction="Column">
				<span>{getUsDate(creationDate)}</span>
				<span>{getUsTime(creationDate)}</span>
			</Flex>
			<Cell
				cellStyles={{
					gridColumn: "1/3",
				}}>
				<span>{longDescription}</span>
			</Cell>
		</Grid>
	);
}

export default IdeaListEntry;