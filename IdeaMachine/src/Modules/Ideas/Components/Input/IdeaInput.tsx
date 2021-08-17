// Framework
import * as React from "react";
import classNames from "classnames";
import { Button, TextField } from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";
import { RouteComponentProps, withRouter } from "react-router-dom";

// Components
import { Grid, Cell } from "common/components";
import TagDisplay from "./TagDisplay";
import Card from "../Card";
import UploadRow from "./UploadRow";
import Separator from "common/components/Controls/Separator";

// Functionality
import { useTranslations } from 'common/hooks/useTranslations';
import useServices from "common/hooks/useServices";

// Types
import { Idea } from 'modules/Ideas/types';

// Styles
import styles from "./styles/IdeaInput.module.less";

type Props = RouteComponentProps;

const customTextFieldStyles = makeStyles((_) => ({
	custom: {
		"& .MuiInputBase-input": {
			color: "white",
		},
		"& .MuiFormLabel-root": {
			color: "white",
		},
		"& .MuiInput-underline:before": {
			borderBottomColor: "black",
		},
	},
}));

export const IdeaInput: React.FC<Props> = ({ history }) => {

	const customStyles = customTextFieldStyles();

	const [idea, setIdea] = React.useState<Idea>({
		longDescription: "",
		shortDescription: "",
	} as Idea);
	const translations = useTranslations();

	const [tags, setTags] = React.useState<Array<string>>([]);

	const { IdeaService } = useServices();

	const onClick = async () => {
		await IdeaService.addIdea(idea);
		history.replace("/");
	}

	return (
		<Card>
			<Grid
				className={styles.IdeaInput}
				gridProperties={{
					gridTemplateRows: "1fr 1fr 1fr 1fr 1fr 1fr",
					gridTemplateColumns: "repeat(4, 1fr)",
					gridTemplateAreas: `
						"Intro Intro Intro Intro Intro"
						"Separator Separator Separator Separator Separator"
						"ShortDescription ShortDescription ShortDescription ShortDescription ShortDescription"
						"LongDescription LongDescription LongDescription LongDescription LongDescription"
						"UploadRow UploadRow UploadRow Tags Tags"
						"Upload . . . Submit"
					`,
				}}>
				<Cell
					cellStyles={{
						gridArea: "Intro",
					}}>
					<span>{translations.AddIdea}</span>
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "Separator",
					}}>
					<Separator
						className={styles.Separator}
						direction="Horizontal"
					/>
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "ShortDescription",
					}}>
					<TextField
						label={translations.AddIdeaShortDescription}
						className={classNames(styles.ShortDescription, customStyles.custom)}
						value={idea.shortDescription}
						color="primary"
						variant="outlined"
						onChange={val => setIdea({
							...idea,
							shortDescription: val.currentTarget.value,
						})} />
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "LongDescription",
					}}>
					<TextField
						label={translations.AddIdeaLongDescription}
						className={classNames(styles.LongDescription, customStyles.custom)}
						value={idea.longDescription}
						color="primary"
						rows={10}
						multiline
						variant="outlined"
						onChange={event => setIdea({
							...idea,
							longDescription: event.currentTarget.value,
						})} />
				</Cell>
				<Cell
					className={styles.TagsArea}
					cellStyles={{
						gridArea: "Tags",
					}}>
					<TagDisplay
						customStyleSet={customStyles}
						tags={tags}
						setTags={setTags} />
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "UploadRow",
					}}>
					<UploadRow />
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "Upload",
					}}>
					<Button
						variant="contained"
						color="primary"
						onClick={onClick}>
						Upload
					</Button>
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "Submit",
					}}>
					<Button
						variant="contained"
						color="primary"
						onClick={onClick}>
						Submit
					</Button>
				</Cell>
			</Grid>
		</Card>
	);
}

export default withRouter(IdeaInput);