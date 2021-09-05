// Framework
import * as React from "react";

// Components
import MaterialIcon from "common/components/MaterialIcon";
import Flex from "common/components/Flex";

// Functionality
import useServices from "common/hooks/useServices";
import openUploadModal from "./UploadModal";

// Types
import { AttachmentUrl } from "modules/Ideas/types";

// Styles
import styles from "./styles/UploadRow.module.less";

type Props = {
	attachments: Array<AttachmentUrl>;
	ideaId: number;
	isOwned?: boolean;
}

export const UploadRow: React.FC<Props> = ({ attachments, ideaId, isOwned = false }) => {
	return (
		<Flex
			className={styles.UploadRow}>
			{
				attachments.map(x => (
					<Attachment
						key={x.attachmentUrl}
						attachment={x}
						ideaId={ideaId}
						isOwned={isOwned} />
				))
			}
		</Flex>
	);
}

const Attachment = ({ attachment, ideaId, isOwned }: { attachment: AttachmentUrl, ideaId: number, isOwned: boolean }) => {

	const { IdeaService } = useServices();

	const onDelete = async () => {
		await IdeaService.deleteAttachment(ideaId, attachment.id);
	}

	return (
		<div className={styles.AttachmentContainer}>
			<img
				className={styles.Image}
				key={attachment.attachmentUrl}
				onClick={() => openUploadModal(attachment.attachmentUrl, onDelete, isOwned)}
				src={attachment.attachmentUrl} />
			<If condition={isOwned}>
				<MaterialIcon
					className={styles.DeleteButton}
					iconName="delete"
					onClick={onDelete}
				/>
			</If>
		</div>
	)
}

export default UploadRow;