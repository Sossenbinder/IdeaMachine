// Framework
import * as React from "react";

// Components
import { Flex } from "common/components";
import { BlackSpinner } from "common/components/controls/Spinner";
import Comment from "./Comment";
import StyledTextField from "../input/StyledTextField";
import StyledButton from "../input/StyledButton";

// Functionality
import useServices from "common/hooks/useServices";
import useAsyncCall from "common/hooks/useAsyncCall";
import useAccount from "common/hooks/useAccount";

// Types
import { Idea } from "modules/ideas/types";

// Styles
import styles from "./styles/CommentSection.module.less";

type Props = {
	idea: Idea;
}

export const CommentSection: React.FC<Props> = ({ idea }) => {

	const { CommentsService } = useServices();
	const [loading, call] = useAsyncCall();
	const account = useAccount();

	const [comment, setComment] = React.useState("");

	const addComment = () => call(() => CommentsService.addComment(idea.id, comment));

	React.useEffect(() => {
		if (!idea.comments) {
			CommentsService.queryComments(idea.id);
		}
	}, [idea]);

	return (
		<Flex
			className={styles.CommentSection}
			direction="Column">
			<div className={styles.CommentList}>
				{
					idea.comments?.map(x => (
						<Comment 
							comment={x}
							key={x.id} />
					))
				}
			</div>
			<Flex 
				className={styles.CommentInputSection}
				crossAlign="Center"
				direction="Row">
				<StyledTextField
					className={styles.Input}
					color="primary"
					label="Your comment" 
					value={comment}					
					onChange={e => setComment(e.currentTarget.value)} />
				<StyledButton
					className={styles.SendButton}
					color={"primary"}
					disabled={account.isAnonymous}
					title={account.isAnonymous ? "Not allowed for anonymous accounts" : ""}
					size={"medium"}
					variant={"contained"}
					onClick={async () => await addComment()}>
					<If condition={!loading}>
						Send
					</If>
					<If condition={loading}>
						<BlackSpinner
							size={50} />
					</If>
				</StyledButton>
			</Flex>
		</Flex>
	);
}

export default CommentSection;