import * as React from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { Grid, Cell } from "common/components";
import TagDisplay from "./TagDisplay";
import UploadRow from "./UploadRow";
import { useTranslations } from "common/hooks/useTranslations";
import useServices from "common/hooks/useServices";
import { AttachmentModel, Idea, IdeaInputResult } from "modules/ideas/types";
import styles from "./styles/IdeaInput.module.scss";
import { Button, Divider, Textarea, TextInput } from "@mantine/core";
import ThemedText from "common/components/ThemedText";

type FileInfo = {
	file: File;
	fileUrl: string;
};

type Errors = {
	shortDescriptionMissing: boolean;
	longDescriptionMissing: boolean;
};

type Props = RouteComponentProps;

export const IdeaInput: React.FC<Props> = ({ history }) => {
	const translations = useTranslations();

	const { IdeaService } = useServices();

	const [idea, setIdea] = React.useState<Idea>({
		longDescription: "",
		shortDescription: "",
	} as Idea);

	const [tags, setTags] = React.useState<Array<string>>([]);

	const [errors, setErrors] = React.useState<Errors>({
		longDescriptionMissing: false,
		shortDescriptionMissing: false,
	});

	const [fileInfos, setFileInfos] = React.useState<Array<FileInfo>>([]);

	const onClick = async () => {
		const files = fileInfos.map((x) => x.file);

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

	const onAttachmentsAdded = (files: Array<File>) => {
		setFileInfos((existingFileInfos) => [
			...existingFileInfos,
			...files.map((file) => ({
				file,
				fileUrl: URL.createObjectURL(file),
			})),
		]);
	};

	const onAttachmentRemoved = (index: number) => {
		setFileInfos((existingFileInfos) => {
			const newFiles = [...existingFileInfos];
			newFiles.splice(index, 1);
			return newFiles;
		});
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
					"UploadRow UploadRow UploadRow . . Submit"
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
				<TagDisplay tags={tags} setTags={setTags} maximumTags={4} />
			</Cell>
			<Cell gridArea="UploadRow">
				<UploadRow
					attachments={fileInfos.map((x) => ({ attachmentUrl: x.fileUrl } as AttachmentModel))}
					onAttachmentsAdded={onAttachmentsAdded}
					onAttachmentRemoved={onAttachmentRemoved}
					canDelete={true}
				/>
				<ThemedText>(Upload attachments here)</ThemedText>
			</Cell>
			<Cell gridArea="Submit">
				<Button variant="filled" onClick={onClick}>
					Submit
				</Button>
			</Cell>
		</Grid>
	);
};

export default withRouter(IdeaInput);
