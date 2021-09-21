// Framework
import * as React from "react";
import { Button, TextField } from "@material-ui/core";

// Components
import { Cell, Flex } from "common/components";

// Functionality
import useServices from "common/hooks/useServices";

// Types
import { Idea } from "modules/ideas/types";

// Styles
import styles from "./styles/CommentSection.module.less";

type Props = {
	idea: Idea;
}

export const CommentSection: React.FC<Props> = ({ idea }) => {

	const { } = useServices();

	return (
		<Flex
			className={styles.CommentSection}
			direction="Column">
			<div className={styles.CommentList}>
				{
					idea.comments?.map(x => {
						<p key={x.commentId}>
							{x.comment}
						</p>
					})
				}
			</div>
			<Flex 
				className={styles.CommentInputSection}
				direction="Row">
				<TextField
					className={styles.Input}
					color="primary"
					label="Your comment" />
				<Button
					className={styles.SubmitButton}
					color={"primary"}
					size={"medium"}
					variant={"contained"}
					onClick={void 0}>
					Send
				</Button>
			</Flex>
		</Flex>
	);
}

export default CommentSection;