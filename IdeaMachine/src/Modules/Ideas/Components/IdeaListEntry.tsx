// Framework
import * as React from "react";
import classNames from "classnames";
import { RouteComponentProps, withRouter } from "react-router-dom";

// Components
import { Grid, Cell, Flex } from "common/components";
import MaterialIcon, { MaterialIconType } from "common/components/MaterialIcon";
import { FeedbackMaterialIcon } from "common/components/FeedbackMaterialIcon";
import Separator from "common/components/Controls/Separator";

// Functionality
import { getUsDate, getUsTime } from "common/utils/timeUtils";
import useServices from "common/hooks/useServices";

// Types
import { LikeState } from "modules/Reaction/types";
import { Idea } from "../types";

// Styles
import styles from "./styles/IdeaListEntry.module.less";

type Props = RouteComponentProps & {
	idea: Idea;
}

export const IdeaListEntry: React.FC<Props> = ({ idea: { shortDescription, creationDate, longDescription, id, ideaReactionMetaData: { totalLike, ownLikeState } }, history }) => {

	const [previewOpen, setPreviewOpen] = React.useState(false);

	const { ReactionService, IdeaService } = useServices();

	const containerClassNames = classNames(
		styles.Container,
		{ [styles.Open]: previewOpen }
	)

	const navTo = (event: React.MouseEvent<HTMLSpanElement, MouseEvent>, navLink: string) => {
		event.stopPropagation();
		event.preventDefault();

		history.push(navLink);
	}

	const modifyLikeState = async (event: React.MouseEvent<HTMLSpanElement, MouseEvent>, likeState: LikeState) => {
		event.stopPropagation();
		event.preventDefault();
		await ReactionService.modifyLike(id, likeState);
	}

	const deleteIdea = async (event: React.MouseEvent<HTMLSpanElement, MouseEvent>) => {
		event.stopPropagation();
		event.preventDefault();
		await IdeaService.deleteIdea(id);
	}

	return (
		<div
			className={containerClassNames}
			onClick={() => setPreviewOpen(!previewOpen)}>
			<Grid
				className={styles.Idea}
				gridProperties={{
					gridTemplateColumns: "30px 9fr 75px 1fr 40px",
					gridTemplateRows: "1fr 1fr"
				}}>
				<Cell
					cellStyles={{
						gridRow: "1/3"
					}}>
					<Flex
						className={styles.TotalLikeContainer}
						direction="Row"
						crossAlign="Center">
						<Flex
							space="Around"
							direction="Column">
							<MaterialIcon
								className={styles.ThumbButton}
								onClick={async ev => await modifyLikeState(ev, LikeState.Like)}
								iconName="thumb_up"
								size={14}
								color={ownLikeState === LikeState.Like ? "blue" : "black"} />
							{`${totalLike >= 0 ? "+" : ""}${totalLike}`}
							<MaterialIcon
								className={styles.ThumbButton}
								onClick={async ev => await modifyLikeState(ev, LikeState.Dislike)}
								iconName="thumb_down"
								size={14}
								color={ownLikeState === LikeState.Dislike ? "blue" : "black"} />
						</Flex>
						<Separator
							direction="Vertical"
							width="20px" />
					</Flex>
				</Cell>
				<span className={styles.ShortDescription}>
					{shortDescription}
				</span>
				<Flex
					className={styles.ControlSection}
					direction="Row">
					<MaterialIcon
						onClick={async event => navTo(event, `/idea/${id}`)}
						iconName="info"
						type={MaterialIconType.Outlined}
						size={25} />
					<MaterialIcon
						onClick={async event => navTo(event, `/idea/${id}/reply`)}
						iconName="reply"
						size={25} />
					<FeedbackMaterialIcon
						onClick={async event => await deleteIdea(event)}
						iconName="delete"
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
							gridColumn: "2/6",
						}}>
						<span>{longDescription}</span>
					</Cell>
				</If>
			</Grid>
		</div>
	);
}

export default withRouter(IdeaListEntry);