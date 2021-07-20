// Framework
import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps, withRouter } from "react-router-dom";

// Components
import IdeaNotFound from "modules/Ideas/Components/IdeaNotFound";
import { Cell, Flex, Grid } from "common/Components";

// Functionality
import { ReduxStore } from "common/Redux/store";
import useAsyncCall from "common/Hooks/useAsyncCall";
import useServices from "common/Hooks/useServices";
import useTranslations from "common/Hooks/useTranslations";

// Functionality
import { getUsDate, getUsTime } from "common/utils/timeUtils";
import { modifyLike } from "modules/Reaction/Communication/ReactionCommunication";

// Types
import { Idea } from "../types";
import { LikeState } from "modules/Reaction/types";

// Styles
import styles from "./Styles/IdeaInfo.module.less";
import MaterialIcon from "common/Components/MaterialIcon";
import CommentSection from "./Info/CommentSection";

type Props = RouteComponentProps<{
	id: string,
}>;

type ReduxProps = {
	idea: Idea;
}

export const IdeaInfo: React.FC<Props & ReduxProps> = ({ match: { params: { id } }, idea, history }) => {

	const idParsed = Number(id);

	const { IdeaService, ReactionService } = useServices();

	const translations = useTranslations();

	const [loading, runCall] = useAsyncCall();

	React.useEffect(() => {
		if (!idea) {
			runCall(() => IdeaService.getSpecificIdea(idParsed));
		}
	}, []);

	return (
		<div className={styles.IdeaInfoContainer}>
			<Choose>
				<When condition={!!idea}>
					<Grid
						className={styles.IdeaInfo}
						gridProperties={{
							gridTemplateColumns: "9fr 1fr 2fr",
							gridTemplateRows: "50px 5fr minmax(0px, 4fr)",
							rowGap: "20px"
						}}>
						<Cell>
							<h2>
								<u>
									{idea.shortDescription}
								</u>
							</h2>
						</Cell>
						<MaterialIcon
							onClick={() => history.push(`/idea/${id}/reply`)}
							iconName="reply"
							color="white"
							size={40} />
						<Flex
							direction="Column">
							<span>{getUsDate(idea.creationDate)}</span>
							<span>{getUsTime(idea.creationDate)}</span>
						</Flex>
						<Cell
							cellStyles={{
								gridColumn: "1/3",
							}}>
							{idea.longDescription}
						</Cell>
						<Flex
							className={styles.ReactionSection}
							direction="Column"
							crossAlign="Center">
							<MaterialIcon
								className={styles.ThumbButton}
								onClick={async _ => await ReactionService.modifyLike(idea.id, LikeState.Like)}
								iconName="thumb_up"
								size={25}
								color={idea.ideaReactionMetaData.ownLikeState === LikeState.Like ? "lightblue" : "black"} />
							<span>
								{`${idea.ideaReactionMetaData.totalLike >= 0 ? "+" : ""}${idea.ideaReactionMetaData.totalLike}`}
							</span>
							<MaterialIcon
								className={styles.ThumbButton}
								onClick={async _ => await ReactionService.modifyLike(idea.id, LikeState.Dislike)}
								iconName="thumb_down"
								size={25}
								color={idea.ideaReactionMetaData.ownLikeState === LikeState.Dislike ? "lightblue" : "black"} />
						</Flex>
						<Cell
							cellStyles={{
								gridColumn: "1/4",
							}}>
							<CommentSection
								idea={idea} />
						</Cell>
					</Grid>
				</When>
				<Otherwise>
					<Choose>
						<When condition={!loading}>
							<IdeaNotFound />
						</When>
						<Otherwise>
							Fetching...
						</Otherwise>
					</Choose>
				</Otherwise>
			</Choose>
		</div>
	);
}

const mapStateToProps = (state: ReduxStore, ownProps: Props): ReduxProps => {
	const idParsed = Number(ownProps.match.params.id);

	return {
		idea: state.ideaReducer.data.find(x => x.id === idParsed),
	}
}

export default connect(mapStateToProps)(withRouter(IdeaInfo));