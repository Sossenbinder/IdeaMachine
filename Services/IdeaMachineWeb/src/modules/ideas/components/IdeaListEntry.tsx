import { Group, Text } from "@mantine/core";
import * as React from "react";
import classNames from "classnames";
import { RouteComponentProps, withRouter } from "react-router-dom";
import Chip from "@mui/material/Chip";
import { Grid, Cell, Flex } from "common/components";
import MaterialIcon from "common/components/MaterialIcon";
import { FeedbackMaterialIcon } from "common/components/FeedbackMaterialIcon";
import { IdeaFilterContext } from "modules/ideas/components/IdeaFilterContext";
import Separator from "common/components/controls/Separator";
import { getUsDate, getUsTime } from "common/utils/timeUtils";
import useServices from "common/hooks/useServices";
import { Idea } from "../types";
import styles from "./styles/IdeaListEntry.module.scss";
import Voting from "./input/Voting";

type Props = RouteComponentProps & {
	idea: Idea;
};

export const IdeaListEntry: React.FC<Props> = ({
	idea: { shortDescription, creationDate, longDescription, id, tags, attachments: attachmentUrls, ideaReactionMetaData },
	history,
}) => {
	const [previewOpen, setPreviewOpen] = React.useState(false);

	const { filters, updateFilters } = React.useContext(IdeaFilterContext);

	const { IdeaService } = useServices();

	const containerClassNames = classNames(styles.Container, { [styles.Open]: previewOpen });

	const navTo = (event: React.MouseEvent<HTMLSpanElement, MouseEvent>, navLink: string) => {
		event.stopPropagation();
		event.preventDefault();

		history.push(navLink);
	};

	const deleteIdea = async (event: React.MouseEvent<HTMLSpanElement, MouseEvent>) => {
		event.stopPropagation();
		event.preventDefault();
		await IdeaService.deleteIdea(id);
	};

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
	};

	return (
		<div className={containerClassNames} onClick={() => setPreviewOpen(!previewOpen)}>
			<Grid
				className={styles.Idea}
				gridTemplateColumns="30px 7fr 100px 1fr 40px"
				gridTemplateRows="1fr 1fr"
				gridTemplateAreas={`
					"TotalLike ShortDescription Actions Timestamp Expand"
					"TotalLike LongDescription Tags Tags ."
				`}
			>
				<Cell gridArea="TotalLike">
					<Group className={styles.TotalLikeContainer} direction="row" align="center" noWrap sx={{ gap: 0 }}>
						<Voting id={id} ideaReactionMetaData={ideaReactionMetaData} />
						<Separator direction="Vertical" width="20px" />
					</Group>
				</Cell>
				<Text className={styles.ShortDescription}>{shortDescription}</Text>
				<Group className={styles.ControlSection} direction="row" align="flex-start" noWrap sx={{ gap: "0" }}>
					<If condition={attachmentUrls && attachmentUrls.length > 0}>
						<Group direction="row" align="center" className={styles.Attachments}>
							<Text className={styles.Number}>{attachmentUrls.length}</Text>
							<MaterialIcon
								className={styles.AttachmentIcon}
								onClick={async (event) => navTo(event, `/idea/${id}`)}
								iconName="attachment"
								size={25}
							/>
						</Group>
					</If>
					<MaterialIcon onClick={(event) => navTo(event, `/idea/${id}`)} iconName="info" type="Outlined" size={25} />
					<MaterialIcon onClick={(event) => navTo(event, `/idea/${id}/reply`)} iconName="share" size={25} />
					<FeedbackMaterialIcon onClick={async (event) => await deleteIdea(event)} iconName="delete" size={25} />
				</Group>
				<Flex direction="Column">
					<Text>{getUsDate(creationDate)}</Text>
					<Text>{getUsTime(creationDate)}</Text>
				</Flex>
				<MaterialIcon className={styles.ExpandMore} iconName={`expand_${previewOpen ? "less" : "more"}`} size={40} />
				<If condition={previewOpen}>
					<Cell gridArea="LongDescription">
						<span>{longDescription}</span>
					</Cell>
					<Cell gridArea="Tags">
						<Flex className={styles.Chips} direction="Row" wrap="Wrap">
							{tags.map((data, index) => (
								<Chip label={data} color="info" key={`Tags_${index}`} onClick={(event) => onTagClick(event, data)} />
							))}
						</Flex>
					</Cell>
				</If>
			</Grid>
		</div>
	);
};

export default withRouter(IdeaListEntry);
