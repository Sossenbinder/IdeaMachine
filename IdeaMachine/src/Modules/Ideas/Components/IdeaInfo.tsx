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

	const { IdeaService } = useServices();

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
							gridTemplateColumns: "9fr 2fr 2fr",
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
						<Cell>
							<Flex
								direction="Column">
								<MaterialIcon
									onClick={() => history.push(`/idea/${id}/reply`)}
									iconName="reply"
									color="white"
									size={40} />
								<MaterialIcon
									onClick={async () => await modifyLike(+id, LikeState.Like)}
									iconName="thumb_up"
									color="white"
									size={40} />
							</Flex>
						</Cell>
						<Cell className={styles.TimeSection}>
							<Flex
								direction="Column">
								<span>{getUsDate(idea.creationDate)}</span>
								<span>{getUsTime(idea.creationDate)}</span>
							</Flex>
						</Cell>
						<Cell
							cellStyles={{
								gridColumn: "1/4",
							}}>
							{idea.longDescription}
						</Cell>
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