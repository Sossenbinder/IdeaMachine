// Framework
import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";

// Components
import IdeaNotFound from "modules/Ideas/Components/IdeaNotFound";
import { Cell, Flex, Grid } from "common/Components";

// Functionality
import { ReduxStore } from "common/Redux/store";
import useAsyncCall from "common/Hooks/useAsyncCall";
import useServices from "common/Hooks/useServices";
import useTranslations from "common/Hooks/useTranslations";

// Types
import { Idea } from "../types";

// Functionality
import { getUsDate, getUsTime } from "common/utils/timeUtils";

// Styles
import styles from "./Styles/IdeaInfo.module.less";

type Props = RouteComponentProps<{
	id: string,
}>;

type ReduxProps = {
	idea: Idea;
}

export const IdeaInfo: React.FC<Props & ReduxProps> = ({ match: { params: { id } }, idea }) => {

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
		<Choose>
			<When condition={!!idea}>
				<Grid
					className={styles.IdeaInfo}
					gridProperties={{
						gridTemplateColumns: "9fr 1fr 2fr",
						gridTemplateRows: "50px 22px 100px 22px 50px",
						rowGap: "20px"
					}}>
					<Cell>
						<h2><u>{idea.shortDescription}</u></h2>
					</Cell>
					<Cell className={styles.TimeSection}>
						<Flex
							direction="Column">
							<span>{getUsDate(idea.creationDate)}</span>
							<span>{getUsTime(idea.creationDate)}</span>
						</Flex>
					</Cell>
					<Cell>
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
	);
}

const mapStateToProps = (state: ReduxStore, ownProps: Props): ReduxProps => {
	const idParsed = Number(ownProps.match.params.id);

	return {
		idea: state.ideaReducer.data.find(x => x.id === idParsed),
	}
}

export default connect(mapStateToProps)(IdeaInfo);