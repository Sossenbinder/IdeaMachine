// Framework
import { render, unmountComponentAtNode } from "react-dom";
import * as React from "react";

// Components
import { Dialog } from "@material-ui/core";

type Props = {
	file: string;
	onExit(): void;
}

const UploadModal: React.FC<Props> = ({ file, onExit, }) => {
	return (
		<Dialog
			open={true}
			onClose={onExit}
		>
			<img src={file} />
		</Dialog>
	);
}

export const openModal = (file: string) => {
	const localDiv = document.createElement("div");

	render(
		<UploadModal
			file={file}
			onExit={() => unmountComponentAtNode(localDiv)} />,
		localDiv);
}

export default openModal;