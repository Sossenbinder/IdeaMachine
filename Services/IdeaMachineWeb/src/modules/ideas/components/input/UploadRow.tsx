import * as React from "react";
import MaterialIcon from "common/components/MaterialIcon";
import openUploadModal from "./UploadModal";
import { AttachmentModel } from "modules/ideas/types";
import styles from "./styles/UploadRow.module.scss";
import useAsyncCall from "common/hooks/useAsyncCall";
import { CirclePlus } from "tabler-icons-react";
import { ActionIcon, Group, Loader } from "@mantine/core";

type Props = {
	attachments: Array<AttachmentModel>;
	onAttachmentsAdded(newAttachments: Array<File>): void;
	onAttachmentRemoved(index: number): void;
	canDelete?: boolean;
};

export const UploadRow = ({ onAttachmentsAdded, onAttachmentRemoved, attachments, canDelete = false }: Props) => {
	const uploadFileRef = React.useRef<HTMLInputElement>(null);

	const [running, call] = useAsyncCall();

	const onFileInputChange: React.ChangeEventHandler<HTMLInputElement> = (_) => {
		const files = uploadFileRef.current.files;

		if (files.length !== 0) {
			call(async () => await onAttachmentsAdded(Array.from(files)));
		}
	};

	return (
		<Group className={styles.UploadRow}>
			{attachments.map((x, index) => (
				<Attachment key={x.attachmentUrl} attachment={x} canDelete={canDelete} onAttachmentRemoved={() => onAttachmentRemoved(index)} />
			))}
			<input type="file" ref={uploadFileRef} onChange={onFileInputChange} accept="image/*" multiple hidden />
			<Group className={styles.UploadButton} align="center">
				{running ? (
					<Loader />
				) : (
					<ActionIcon size="xl" variant="transparent" onClick={() => uploadFileRef.current.click()}>
						<CirclePlus size={75} />
					</ActionIcon>
				)}
			</Group>
		</Group>
	);
};

const Attachment = ({ attachment, canDelete, onAttachmentRemoved }: { attachment: AttachmentModel; canDelete: boolean; onAttachmentRemoved(): void }) => {
	const onImageClick = () => {
		openUploadModal(attachment.attachmentUrl);
	};

	return (
		<div className={styles.AttachmentContainer}>
			<img className={styles.Image} key={attachment.attachmentUrl} onClick={onImageClick} src={attachment.attachmentUrl} />
			{canDelete && <MaterialIcon className={styles.DeleteButton} iconName="delete" onClick={async () => await onAttachmentRemoved()} />}
		</div>
	);
};

export default UploadRow;
