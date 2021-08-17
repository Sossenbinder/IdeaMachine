// Framework
import * as React from "react";
import { ImageList, ImageListItem } from "@material-ui/core";

// Components
import Flex from "common/components/Flex";

// Functionality

// Types

// Styles
import styles from "./styles/UploadRow.module.less";

type Props = {

}

export const UploadRow: React.FC<Props> = () => {
	return (
		<ImageList>
			<ImageListItem>
				<img src="/Resources/Images/Background.jpg" />
			</ImageListItem>
		</ImageList>
	);
}

export default UploadRow;