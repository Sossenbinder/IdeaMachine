// Framework
import * as React from "react";
import { TextareaAutosize, Button } from "@material-ui/core";
import { RouteComponentProps, withRouter } from "react-router-dom";

// Components
import { Grid, Cell } from "common/Components";

// Functionality
import { useTranslations } from 'common/Hooks/useTranslations';
import useServices from "common/Hooks/useServices";

// Types
import { Idea } from 'modules/Ideas/types';

// Styles
import styles from "./Styles/IdeaInput.module.less";

type Props = RouteComponentProps;

export const IdeaInput: React.FC<Props> = ({ history }) => {

	const [idea, setIdea] = React.useState<Idea>({
		longDescription: "",
		shortDescription: "",
	} as Idea);

	const translations = useTranslations();

	const { IdeaService } = useServices();

	const onClick = async () => {
		await IdeaService.addIdea(idea);
		history.replace("/");
	}

	return (
		<Grid
			className={styles.IdeaInput}
			gridProperties={{
				gridTemplateColumns: "1fr 3fr 50px",
				gridTemplateRows: "50px 22px 100px 22px 50px",
				rowGap: "20px"
			}}>
			<Cell
				cellStyles={{
					gridColumn: "1/4"
				}}>
				<span>{translations.AddIdea}</span>
			</Cell>

			<span>{translations.AddIdeaShortDescription}</span>
			<Cell
				cellStyles={{
					gridColumn: "2/4"
				}}>
				<input
					type={"text"}
					value={idea.shortDescription}
					onChange={val => setIdea({
						...idea,
						shortDescription: val.currentTarget.value,
					})} />
			</Cell>
			<span>{translations.AddIdeaLongDescription}</span>
			<Cell
				cellStyles={{
					gridColumn: "2/4"
				}}>
				<TextareaAutosize
					rowsMin={5}
					onChange={event => setIdea({
						...idea,
						longDescription: event.currentTarget.value,
					})} />
			</Cell>
			<Cell
				cellStyles={{
					gridColumn: "2/4"
				}}>
				<Button
					variant="contained"
					color="primary"
					onClick={onClick}>
					Submit
				</Button>
			</Cell>
		</Grid>
	);
}

export default withRouter(IdeaInput);