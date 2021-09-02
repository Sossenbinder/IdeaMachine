// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";

// Functionality
import openModal from "./UploadModal";

// Types

// Styles
import styles from "./styles/UploadRow.module.less";

type Props = {
	fileUrls: Array<string>;
}

export const UploadRow: React.FC<Props> = ({ fileUrls }) => {
	return (
		<Flex
			className={styles.UploadRow}>
			{
				fileUrls.map(x => (
					<img
						className={styles.Image}
						key={x}
						onClick={() => openModal(x)}
						src={x} />
				))
			}
		</Flex>
	);
}

export default UploadRow;