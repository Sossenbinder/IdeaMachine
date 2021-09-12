// Framework
import { render, unmountComponentAtNode } from "react-dom";
import * as React from "react";

// Components
import { Dialog } from "@material-ui/core";

type Props = {
	file: string;
	onExit(): void;
	isOwned: boolean;
	onDelete(): Promise<void>;
}

const UploadModal: React.FC<Props> = ({ file, onExit, isOwned }) => {
	return (
		<Dialog
			open={true}
			onClose={onExit}
		>
			<img src={file} />
		</Dialog>
	); 
}

export const openUploadModal = (file: string, onDelete: () => Promise<void>, isOwned?: boolean) => {
	const localDiv = document.createElement("div");

	render(
		<UploadModal
			file={file}
			onDelete={onDelete}
			onExit={() => unmountComponentAtNode(localDiv)}
			isOwned={isOwned} />,
		localDiv);
}

export default openUploadModal;