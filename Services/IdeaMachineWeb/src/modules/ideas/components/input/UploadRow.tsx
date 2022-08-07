// Framework
import * as React from "react";

// Components
import MaterialIcon from "common/components/MaterialIcon";
import Flex from "common/components/Flex";

// Functionality
import useServices from "common/hooks/useServices";
import openUploadModal from "./UploadModal";

// Types
import { AttachmentUrl } from "modules/ideas/types";

// Styles
import styles from "./styles/UploadRow.module.scss";
import useAsyncCall from "common/hooks/useAsyncCall";
import CircularProgress from "@mui/material/CircularProgress";
import { CirclePlus } from "tabler-icons-react";
import { ActionIcon } from "@mantine/core";

type Props = {
	attachments: Array<AttachmentUrl>;
	onAttachmentAdded(newAttachment: File): void;
	ideaId: number;
	isOwned?: boolean;
};

export const UploadRow: React.FC<Props> = ({ attachments, ideaId, isOwned = false }) => {
	const uploadFileRef = React.useRef<HTMLInputElement>(null);

	const [running, call] = useAsyncCall();

	const { IdeaService } = useServices();

	const onFileInputChange: React.ChangeEventHandler<HTMLInputElement> = (_) => {
		const files = uploadFileRef.current.files;

		if (files.length !== 0) {
			call(() => IdeaService.uploadAttachment(ideaId, files[0]));
		}
	};

	return (
		<Flex className={styles.UploadRow}>
			{attachments.map((x) => (
				<Attachment key={x.attachmentUrl} attachment={x} ideaId={ideaId} isOwned={isOwned} />
			))}
			<If condition={isOwned}>
				<input type="file" ref={uploadFileRef} onChange={onFileInputChange} accept="image/*" multiple hidden />
				<Flex className={styles.UploadButton} mainAlign="Center" crossAlign="Center">
					<Choose>
						<When condition={!running}>
							<ActionIcon size="xl" variant="transparent" onClick={() => uploadFileRef.current.click()}>
								<CirclePlus size={75} />
							</ActionIcon>
						</When>
						<Otherwise>
							<CircularProgress />
						</Otherwise>
					</Choose>
				</Flex>
			</If>
		</Flex>
	);
};

const Attachment = ({ attachment, ideaId, isOwned }: { attachment: AttachmentUrl; ideaId: number; isOwned: boolean }) => {
	const { IdeaService } = useServices();

	const onDelete = async () => {
		await IdeaService.deleteAttachment(ideaId, attachment.id);
	};

	const onImageClick = () => {
		openUploadModal(attachment.attachmentUrl, onDelete, isOwned);
	};

	return (
		<div className={styles.AttachmentContainer}>
			<img className={styles.Image} key={attachment.attachmentUrl} onClick={onImageClick} src={attachment.attachmentUrl} />
			<If condition={isOwned}>
				<MaterialIcon className={styles.DeleteButton} iconName="delete" onClick={onDelete} />
			</If>
		</div>
	);
};

export default UploadRow;
