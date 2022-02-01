// Framework
import * as React from "react";

// Components
import { Grid, Cell } from "common/components";

// Functionality
import * as timeUtils from "common/utils/timeUtils";

// Types
import { Comment as CommentType } from "modules/comments/types";

// Styles
import styles from "./styles/Comment.module.scss";

type Props = {
	comment: CommentType;
};

export const Comment = ({ comment: { comment, timeStamp, commenterName } }: Props) => {
	return (
		<div className={styles.CommentContainer}>
			<Grid
				className={styles.Comment}
				gridTemplateAreas={`
						'commenter commenter time'
						'comment comment time'
					`}
				gridTemplateColumns="1fr 8fr 1fr"
				gridTemplateRows="0.9rem minmax(0, 1fr)"
			>
				<Cell gridArea="commenter">
					<span className={styles.CommenterName}>{commenterName}</span>
				</Cell>
				<Cell gridArea="time">
					<span title={`${timeUtils.getUsDate(timeStamp)} - ${timeUtils.getUsTime(timeStamp)}`}>{timeUtils.getFormattedTimeDistance(timeStamp)}</span>
				</Cell>
				<Cell gridArea="comment">
					<p className={styles.Text}>{comment}</p>
				</Cell>
			</Grid>
		</div>
	);
};

export default Comment;
