// Framework
import { render, unmountComponentAtNode } from "react-dom";
import * as React from "react";

// Components
import Dialog from "@mui/material/Dialog";

type Props = {
	file: string;
	onExit(): void;
};

const UploadModal: React.FC<Props> = ({ file, onExit }) => {
	return (
		<Dialog open={true} onClose={onExit}>
			<img src={file} />
		</Dialog>
	);
};

export const openUploadModal = (file: string) => {
	const localDiv = document.createElement("div");

	render(<UploadModal file={file} onExit={() => unmountComponentAtNode(localDiv)} />, localDiv);
};

export default openUploadModal;
