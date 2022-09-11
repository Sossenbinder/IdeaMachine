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
import { sortByDateDesc } from "common/utils/collection";

// Types
import { Idea } from "modules/ideas/types";

// Styles
import styles from "./styles/CommentSection.module.scss";

type Props = {
	idea: Idea;
};

export const CommentSection: React.FC<Props> = ({ idea }) => {
	const { CommentsService } = useServices();
	const [loading, call] = useAsyncCall();
	const account = useAccount();

	const commentlistRef = React.createRef<HTMLDivElement>();

	const [comment, setComment] = React.useState("");

	const addComment = () =>
		call(async () => {
			const success = await CommentsService.addComment(idea.id, comment);

			if (success) {
				setComment("");
			}
		});

	React.useEffect(() => {
		if (!idea.comments) {
			CommentsService.queryComments(idea.id);
		}
	}, [idea]);

	const orderedComments = React.useMemo(() => sortByDateDesc(idea.comments, (x) => x.timeStamp), [idea.comments]);

	React.useEffect(() => {
		if (!commentlistRef.current) {
			return;
		}

		commentlistRef.current.scrollTo(0, 0);
	}, [orderedComments]);

	return (
		<Flex className={styles.CommentSection} direction="Column">
			<div className={styles.CommentList} ref={commentlistRef}>
				{orderedComments.map((x, i) => (
					<Comment comment={x} key={i} />
				))}
			</div>
			<Flex className={styles.CommentInputSection} crossAlign="Center" direction="Row">
				<StyledTextField
					className={styles.Input}
					color="primary"
					label="Your comment"
					value={comment}
					onKeyUp={async (e) => {
						if (e.key === "Enter") {
							await addComment();
						}
					}}
					onChange={(e) => setComment(e.currentTarget.value)}
				/>
				<StyledButton
					className={styles.SendButton}
					color="primary"
					disabled={account.isAnonymous}
					title={account.isAnonymous ? "Not allowed for anonymous accounts" : ""}
					size={"medium"}
					variant={"contained"}
					onClick={async () => await addComment()}
				>
					<If condition={!loading}>Send</If>
					<If condition={loading}>
						<BlackSpinner />
					</If>
				</StyledButton>
			</Flex>
		</Flex>
	);
};

export default CommentSection;
