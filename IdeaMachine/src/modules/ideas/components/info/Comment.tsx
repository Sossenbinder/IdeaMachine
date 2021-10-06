// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";

// Types
import { Comment as CommentType } from "modules/comments/types";

// Styles
import styles from "./styles/Comment.module.less";

type Props = {
    comment: CommentType;
}

export const Comment = ({ comment: { comment, commenterId, timeStamp }}: Props) => {    
    return (
        <Flex
            className={styles.Comment}
            direction="Row"
			space="Between">
            <p>
				{timeStamp}
			</p>
			<p>
				{comment}
			</p>
			<p>
				{commenterId}
			</p>
        </Flex>
    );
}

export default Comment;