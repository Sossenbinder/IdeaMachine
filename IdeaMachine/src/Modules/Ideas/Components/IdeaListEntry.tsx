// Framework
import * as React from "react";
import classNames from "classnames";
import { RouteComponentProps, withRouter } from "react-router-dom";

// Components
import { Grid, Cell, Flex } from "common/Components";
import MaterialIcon, { MaterialIconType } from "common/Components/MaterialIcon";

// Functionality
import { getUsDate, getUsTime } from "common/Utils/timeUtils";

// Types
import { Idea } from "../types";

// Styles
import styles from "./Styles/IdeaListEntry.module.less";

type Props = RouteComponentProps & {
	idea: Idea;
}

export const IdeaListEntry: React.FC<Props> = ({ idea: { shortDescription, creationDate, longDescription, id }, history }) => {

	const [previewOpen, setPreviewOpen] = React.useState(false);

	const containerClassNames = classNames(
		styles.Container,
		{ [styles.Open]: previewOpen }
	)

	const navTo = (event: React.MouseEvent<HTMLSpanElement, MouseEvent>, navLink: string) => {
		event.stopPropagation();
		event.preventDefault();

		history.push(navLink);
	}

	return (
		<div
			className={containerClassNames}
			onClick={() => setPreviewOpen(!previewOpen)}>
			<Grid
				className={styles.Idea}
				gridProperties={{
					gridTemplateColumns: "9fr 50px 1fr 40px",
					gridTemplateRows: "1fr 1fr"
				}}>
				<span className={styles.ShortDescription}>
					{shortDescription}
				</span>
				<Flex
					className={styles.ControlSection}
					direction="Row">
					<MaterialIcon
						onClick={event => navTo(event, `/idea/${id}`)}
						iconName="info"
						type={MaterialIconType.Outlined}
						size={25} />
					<MaterialIcon
						onClick={event => navTo(event, `/idea/${id}/reply`)}
						iconName="reply"
						size={25} />
				</Flex>
				<Flex direction="Column">
					<span>{getUsDate(creationDate)}</span>
					<span>{getUsTime(creationDate)}</span>
				</Flex>
				<MaterialIcon
					className={styles.ExpandMore}
					iconName={`expand_${previewOpen ? "less" : "more"}`}
					size={40} />
				<If condition={previewOpen}>
					<Cell
						cellStyles={{
							gridColumn: "1/5",
						}}>
						<span>{longDescription}</span>
					</Cell>
				</If>
			</Grid>
		</div>
	);
}

export default withRouter(IdeaListEntry);