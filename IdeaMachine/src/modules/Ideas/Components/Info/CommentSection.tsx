// Framework
import * as React from "react";
import { TextField } from "@material-ui/core";

// Components
import { Cell, Flex } from "common/components";

// Functionality

// Types
import { Idea } from "modules/Ideas/types";

// Styles
import styles from "./styles/CommentSection.module.less";

type Props = {
	idea: Idea;
}

export const CommentSection: React.FC<Props> = ({ idea }) => {
	return (
		<Flex
			className={styles.CommentSection}
			direction="Column">
			<div className={styles.CommentList}>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
				<p>{idea.longDescription}</p>
			</div>
			<TextField
				className={styles.CommentInput}
				color="primary"
				label="Your comment" />
		</Flex>
	);
}

export default CommentSection;