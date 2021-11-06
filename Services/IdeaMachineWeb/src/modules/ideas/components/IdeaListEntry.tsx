// Framework
import * as React from "react";
import classNames from "classnames";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { Chip } from "@material-ui/core";

// Components
import { Grid, Cell, Flex } from "common/components";
import MaterialIcon, { MaterialIconType } from "common/components/MaterialIcon";
import { FeedbackMaterialIcon } from "common/components/FeedbackMaterialIcon";
import { IdeaFilterContext } from "modules/ideas/components/IdeaFilterContext";
import Separator from "common/components/controls/Separator";

// Functionality
import { getUsDate, getUsTime } from "common/utils/timeUtils";
import useServices from "common/hooks/useServices";

// Types
import { LikeState } from "modules/reaction/types";
import { Idea } from "../types";

// Styles
import styles from "./styles/IdeaListEntry.module.less";

type Props = RouteComponentProps & {
	idea: Idea;
}

export const IdeaListEntry: React.FC<Props> = ({ idea: { shortDescription, creationDate, longDescription, id, tags, attachmentUrls, ideaReactionMetaData: { totalLike, ownLikeState } }, history }) => {

	const [previewOpen, setPreviewOpen] = React.useState(false);

	const { filters, updateFilters } = React.useContext(IdeaFilterContext);

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

	const onTagClick = (event: React.MouseEvent<HTMLSpanElement, MouseEvent>, tag: string) => {
		event.stopPropagation();
		event.preventDefault();

		if (filters.tags.indexOf(tag) !== -1) {
			return;
		}

		updateFilters({
			...filters,
			tags: [...filters.tags, tag],
		});
	}

	return (
		<div
			className={containerClassNames}
			onClick={() => setPreviewOpen(!previewOpen)}>
			<Grid
				className={styles.Idea}
				gridProperties={{
					gridTemplateColumns: "30px 7fr 100px 1fr 40px",
					gridTemplateRows: "1fr 1fr",
					gridTemplateAreas: `
						"TotalLike ShortDescription Actions Timestamp Expand"
						"TotalLike LongDescription Tags Tags ."
					`,
				}}>
				<Cell
					cellStyles={{
						gridArea: "TotalLike"
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
					direction="Row"
					crossAlign="Start">
					<If condition={attachmentUrls && attachmentUrls.length > 0}>
						<Flex
							direction="Row"
							crossAlign="Center"
							className={styles.Attachments}>
							<span className={styles.Number}>
								{attachmentUrls.length}
							</span>
							<MaterialIcon
								className={styles.AttachmentIcon}
								onClick={async event => navTo(event, `/idea/${id}`)}
								iconName="attachment"
								size={25} />
						</Flex>
					</If>
					<MaterialIcon
						onClick={async event => navTo(event, `/idea/${id}`)}
						iconName="info"
						type="Outlined"
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
							gridArea: "LongDescription",
						}}>
						<span>{longDescription}</span>
					</Cell>
					<Cell
						cellStyles={{
							gridArea: "Tags",
						}}>
						<Flex
							className={styles.Chips}
							direction="Row"
							wrap="Wrap">
							{tags.map((data, index) => {
								return (
									<Chip
										label={data}
										color="info"
										key={`Tags_${index}`}
										onClick={event => onTagClick(event, data)}
									/>
								);
							})}
						</Flex>
					</Cell>
				</If>
			</Grid>
		</div>
	);
}

export default withRouter(IdeaListEntry);