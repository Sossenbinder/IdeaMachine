import * as React from "react";
import Button from "@mui/material/Button";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { Grid, Cell, Flex } from "common/components";
import TagDisplay from "./TagDisplay";
import UploadRow from "./UploadRow";
import MaterialIcon from "common/components/MaterialIcon";
import ValidatableTextField from "./ValidatableTextField";
import { useTranslations } from "common/hooks/useTranslations";
import useServices from "common/hooks/useServices";
import { AttachmentUrl, Idea, IdeaInputResult } from "modules/ideas/types";
import styles from "./styles/IdeaInput.module.scss";
import { Divider, Textarea, TextInput } from "@mantine/core";
import ThemedText from "common/components/ThemedText";

type Errors = {
	shortDescriptionMissing: boolean;
	longDescriptionMissing: boolean;
};

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

		const result = await IdeaService.addIdea(
			{
				...idea,
				tags,
			},
			files,
		);

		if ((result & (IdeaInputResult.MissingShortDescription | IdeaInputResult.MissingLongDescription)) != 0) {
			setErrors({
				longDescriptionMissing: (result & IdeaInputResult.MissingLongDescription) != 0,
				shortDescriptionMissing: (result & IdeaInputResult.MissingShortDescription) != 0,
			});
		}

		if (result & IdeaInputResult.Successful) {
			history.replace("/");
		}
	};

	const onUploadClick = () => {
		uploadFileRef.current.click();
	};

	const onFileInputChange: React.ChangeEventHandler<HTMLInputElement> = (event) => {
		setFileUrls(Array.from(event.target.files).map((x) => URL.createObjectURL(x)));
	};

	return (
		<Grid
			className={styles.IdeaInput}
			gridTemplateRows="20px 50px 1fr 4fr 1fr 65px"
			gridTemplateColumns="repeat(6, 1fr)"
			gridTemplateAreas={`
					"Intro Intro Intro Intro Intro Intro"
					"Separator Separator Separator Separator Separator Separator"
					"ShortDescription ShortDescription ShortDescription ShortDescription ShortDescription ShortDescription"
					"LongDescription LongDescription LongDescription LongDescription LongDescription LongDescription"
					"UploadRow UploadRow UploadRow Tags Tags Tags"
					"Upload Upload . . . Submit"
				`}
		>
			<Cell gridArea="Intro">
				<ThemedText>{translations.AddIdea}</ThemedText>
			</Cell>
			<Cell gridArea="Separator" className={styles.SeparatorContainer}>
				<Divider size="md" />
			</Cell>
			<Cell gridArea="ShortDescription">
				<TextInput
					placeholder={translations.AddIdeaShortDescription}
					className={styles.ShortDescription}
					label="Short description"
					value={idea.shortDescription}
					error={errors.shortDescriptionMissing ? translations.InputMissing : ""}
					onChange={(val) =>
						setIdea({
							...idea,
							shortDescription: val.currentTarget.value,
						})
					}
				/>
			</Cell>
			<Cell gridArea="LongDescription">
				<Textarea
					placeholder={translations.AddIdeaLongDescription}
					className={styles.LongDescription}
					label="Long description"
					value={idea.longDescription}
					error={errors.longDescriptionMissing ? translations.InputMissing : ""}
					onChange={(event) => {
						setIdea({
							...idea,
							longDescription: event.currentTarget.value,
						});
					}}
					styles={{
						wrapper: {
							height: "100%",
						},
						label: {
							height: "20px",
						},
						input: {
							height: "calc(100% - 30px)",
						},
					}}
				/>
			</Cell>
			<Cell className={styles.TagsArea} gridArea="Tags">
				<TagDisplay tags={tags} setTags={setTags} />
			</Cell>
			<Cell gridArea="UploadRow">
				<UploadRow attachments={fileUrls.map((x) => ({ attachmentUrl: x } as AttachmentUrl))} onAttachmentAdded={(_) => void 0} ideaId={idea.id} />
			</Cell>
			<Cell gridArea="Upload">
				<input type="file" name="uploadFile" ref={uploadFileRef} onChange={onFileInputChange} accept="image/*" multiple hidden />
				<Button variant="contained" color="primary" className={styles.UploadAttachmentButton} onClick={onUploadClick}>
					<Flex direction="Row" crossAlign="Center">
						<MaterialIcon color="white" iconName="attach_file" />
						Upload attachment
					</Flex>
				</Button>
			</Cell>
			<Cell gridArea="Submit">
				<Button variant="contained" color="primary" onClick={onClick}>
					Submit
				</Button>
			</Cell>
		</Grid>
	);
};

export default withRouter(IdeaInput);
