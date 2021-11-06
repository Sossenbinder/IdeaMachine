// Framework
import * as React from "react";
import { Button } from "@material-ui/core";
import { RouteComponentProps, withRouter } from "react-router-dom";

// Components
import { Grid, Cell, Flex } from "common/components";
import TagDisplay from "./TagDisplay";
import Card from "../Card";
import UploadRow from "./UploadRow";
import MaterialIcon from "common/components/MaterialIcon";
import Separator from "common/components/controls/Separator";
import ValidatableTextField from "./ValidatableTextField";

// Functionality
import { useTranslations } from 'common/hooks/useTranslations';
import useServices from "common/hooks/useServices";

// Types
import { AttachmentUrl, Idea, IdeaInputResult } from 'modules/ideas/types';

// Styles
import styles from "./styles/IdeaInput.module.less";

type Errors = {
	shortDescriptionMissing: boolean;
	longDescriptionMissing: boolean;
}

type Props = RouteComponentProps;

export const IdeaInput: React.FC<Props> = ({ history }) => {

	const translations = useTranslations();

	const { IdeaService } = useServices();

	const uploadFileRef = React.useRef<HTMLInputElement>(null);

	const [idea, setIdea] = React.useState<Idea>({
		longDescription: "",
		shortDescription: "",
	} as Idea);

	const [tags, setTags] = React.useState<Array<string>>([]);

	const [errors, setErrors] = React.useState<Errors>({
		longDescriptionMissing: false,
		shortDescriptionMissing: false,
	});

	const [fileUrls, setFileUrls] = React.useState<Array<string>>([]);

	const onClick = async () => {

		const files = uploadFileRef.current.files;

		const result = await IdeaService.addIdea({
			...idea,
			tags,
		}, files);

		if ((result & (IdeaInputResult.MissingShortDescription | IdeaInputResult.MissingLongDescription)) != 0) {
			setErrors({
				longDescriptionMissing: (result & IdeaInputResult.MissingLongDescription) != 0,
				shortDescriptionMissing: (result & IdeaInputResult.MissingShortDescription) != 0,
			});
		}

		if (result & IdeaInputResult.Successful) {
			history.replace("/");
		}
	}

	const onUploadClick = () => {
		uploadFileRef.current.click();
	}

	const onFileInputChange: React.ChangeEventHandler<HTMLInputElement> = (event) => {
		setFileUrls(Array.from(event.target.files).map(x => URL.createObjectURL(x)));
	}

	return (
		<Card>
			<Grid
				className={styles.IdeaInput}
				gridProperties={{
					gridTemplateRows: "20px 50px 1fr 4fr 1fr 65px",
					gridTemplateColumns: "repeat(6, 1fr)",
					gridTemplateAreas: `
						"Intro Intro Intro Intro Intro Intro"
						"Separator Separator Separator Separator Separator Separator"
						"ShortDescription ShortDescription ShortDescription ShortDescription ShortDescription ShortDescription"
						"LongDescription LongDescription LongDescription LongDescription LongDescription LongDescription"
						"UploadRow UploadRow UploadRow Tags Tags Tags"
						"Upload Upload . . . Submit"
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
					<ValidatableTextField
						label={translations.AddIdeaShortDescription}
						className={styles.ShortDescription}
						value={idea.shortDescription}
						color="primary"
						variant="outlined"
						error={errors.shortDescriptionMissing}
						helperText={"This input field must not be empty"}
						validate={val => !!val}
						onChange={val => setIdea({
							...idea,
							shortDescription: val.currentTarget.value,
						})}
					/>
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "LongDescription",
					}}>
					<ValidatableTextField
						label={translations.AddIdeaLongDescription}
						className={styles.LongDescription}
						value={idea.longDescription}
						color="primary"
						rows={10}
						multiline
						variant="outlined"
						error={errors.longDescriptionMissing}
						helperText={"This input field must not be empty"}
						validate={val => !!val}
						onChange={event => {
							setIdea({
								...idea,
								longDescription: event.currentTarget.value,
							})
						}} />
				</Cell>
				<Cell
					className={styles.TagsArea}
					cellStyles={{
						gridArea: "Tags",
					}}>
					<TagDisplay
						tags={tags}
						setTags={setTags} />
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "UploadRow",
					}}>
					<UploadRow
						attachments={fileUrls.map(x => ({ attachmentUrl: x }) as AttachmentUrl)}
						onAttachmentAdded={(_) => void 0}
						ideaId={idea.id} />
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "Upload",
					}}>
					<input
						type="file"
						name="uploadFile"
						ref={uploadFileRef}
						onChange={onFileInputChange}
						accept="image/*"
						multiple
						hidden
					/>
					<Button
						variant="contained"
						color="primary"
						className={styles.UploadAttachmentButton}
						onClick={onUploadClick}>
						<Flex
							direction="Row"
							crossAlign="Center">
							<MaterialIcon
								color="white"
								iconName="attach_file"
							/>
							Upload attachment
						</Flex>
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