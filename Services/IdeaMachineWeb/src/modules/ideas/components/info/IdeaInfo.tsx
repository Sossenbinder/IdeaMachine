import * as React from "react";
import { connect, useSelector } from "react-redux";
import { RouteComponentProps, withRouter } from "react-router-dom";
import IdeaNotFound from "modules/ideas/components/IdeaNotFound";
import { Cell, Flex, Grid } from "common/components";
import Card from "../Card";
import MaterialIcon from "common/components/MaterialIcon";
import CommentSection from "./CommentSection";
import UploadRow from "../input/UploadRow";
import { ReduxStore } from "common/redux/store";
import useAsyncCall from "common/hooks/useAsyncCall";
import useServices from "common/hooks/useServices";
import { getUsDate, getUsTime } from "common/utils/timeUtils";
import { AttachmentUrl } from "../../types";
import styles from "./styles/IdeaInfo.module.scss";
import { Text } from "@mantine/core";
import Voting from "../input/Voting";
import useAccount from "common/hooks/useAccount";

type Props = RouteComponentProps<{
	id: string;
}>;

export const IdeaInfo: React.FC<Props> = ({
	match: {
		params: { id },
	},
	history,
}) => {
	const idParsed = Number(id);
	const account = useAccount();

	const idea = useSelector((state: ReduxStore) => state.ideaReducer.data.find((x) => x.id === idParsed));

	const { IdeaService } = useServices();

	const [loading, runCall] = useAsyncCall();

	React.useEffect(() => {
		if (!idea) {
			runCall(() => IdeaService.getSpecificIdea(idParsed));
		}
	}, []);

	return (
		<Card>
			<Choose>
				<When condition={!!idea}>
					<Grid
						className={styles.IdeaInfoGrid}
						gridTemplateColumns="9fr 1fr 2fr"
						gridTemplateRows="50px 4fr 1fr minmax(0px, 4fr)"
						rowGap="20px"
						gridTemplateAreas={`
							"ShortDescription Reply Timestamp"
							"LongDescription LongDescription Rating"
							"Attachments Attachments Attachments"
							"Comments Comments Comments"
						`}
					>
						<Cell gridArea="ShortDescription">
							<h2>
								<u>{idea.shortDescription}</u>
							</h2>
						</Cell>
						<Cell gridArea="Reply">
							<MaterialIcon onClick={() => history.push(`/idea/${id}/reply`)} iconName="reply" color="white" size={40} />
						</Cell>
						<Cell gridArea="Timestamp">
							<Flex direction="Column">
								<span>{getUsDate(idea.creationDate)}</span>
								<span>{getUsTime(idea.creationDate)}</span>
							</Flex>
						</Cell>
						<Cell gridArea="LongDescription">{idea.longDescription}</Cell>
						<Cell gridArea="Rating">
							<Voting id={idParsed} ideaReactionMetaData={idea.ideaReactionMetaData} />
						</Cell>
						<Cell gridArea="Attachments">
							<Text className={styles.Attachments}>Attachments:</Text>
							<UploadRow
								ideaId={idea.id}
								isOwned={idea.creatorId === account.userId}
								attachments={idea.attachmentUrls}
								onAttachmentAdded={(attachment) => {
									const newIdea = { ...idea };
									newIdea.attachmentUrls.push({
										attachmentUrl: URL.createObjectURL(attachment),
									} as AttachmentUrl);
								}}
							/>
						</Cell>
						<Cell gridArea="Comments">
							<CommentSection idea={idea} />
						</Cell>
					</Grid>
				</When>
				<Otherwise>
					<Choose>
						<When condition={!loading}>
							<IdeaNotFound />
						</When>
						<Otherwise>Fetching...</Otherwise>
					</Choose>
				</Otherwise>
			</Choose>
		</Card>
	);
};

export default withRouter(IdeaInfo);
