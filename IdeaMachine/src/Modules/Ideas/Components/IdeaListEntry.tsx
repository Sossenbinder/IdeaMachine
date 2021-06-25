// Framework
import * as React from "react";
import classNames from "classnames";

// Components
import { Grid, Cell, Flex } from "common/Components";
import MaterialIcon from "common/Components/MaterialIcon";

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

	const [previewOpen, setPreviewOpen] = React.useState(false);

	const containerClassNames = classNames(
		styles.Container,
		{ [styles.Open]: previewOpen }
	)

	return (
		<div className={containerClassNames}>
			<Grid
				className={styles.Idea}
				gridProperties={{
					gridTemplateColumns: "9fr 1fr 40px",
					gridTemplateRows: "50px 1fr"
				}}>
				<span>{shortDescription}</span>
				<Flex direction="Column">
					<span>{getUsDate(creationDate)}</span>
					<span>{getUsTime(creationDate)}</span>
				</Flex>
				<MaterialIcon
					onClick={() => setPreviewOpen(!previewOpen)}
					className={styles.ExpandMore}
					iconName={`expand_${previewOpen ? "less" : "more"}`}
					size={40} />
				<If condition={previewOpen}>
					<Cell
						cellStyles={{
							gridColumn: "1/4",
						}}>
						<span>{longDescription}</span>
					</Cell>
				</If>
			</Grid>
		</div>
	);
}

export default IdeaListEntry;