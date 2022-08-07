import * as React from "react";
import { Group, ActionIcon, Text } from "@mantine/core";
import { withSuppressedEvent } from "common/utils/event";
import useServices from "common/hooks/useServices";
import { LikeState } from "modules/reaction/types";
import { IdeaReactionMetaData } from "../../types";
import { ThumbDown, ThumbUp } from "tabler-icons-react";

import styles from "./styles/Voting.module.scss";

type Props = {
	id: number;
	ideaReactionMetaData: IdeaReactionMetaData;
};

export default function Voting({ id, ideaReactionMetaData: { ownLikeState, totalLike } }: Props) {
	const { ReactionService } = useServices();

	const modifyLikeState = (event: React.MouseEvent<HTMLButtonElement>, likeState: LikeState) =>
		withSuppressedEvent(event, async () => {
			await ReactionService.modifyLike(id, likeState);
		});

	const createThumb = (likeState: LikeState) => {
		return (
			<ActionIcon className={styles.VotingThumb} onClick={(event: React.MouseEvent<HTMLButtonElement>) => modifyLikeState(event, likeState)}>
				{likeState === LikeState.Like ? (
					<ThumbUp fill={ownLikeState === LikeState.Like ? "#3482E9" : "white"} />
				) : (
					<ThumbDown fill={ownLikeState === LikeState.Dislike ? "#3482E9" : "white"} />
				)}
			</ActionIcon>
		);
	};

	return (
		<Group position="apart" direction="column" sx={{ gap: 0 }}>
			{createThumb(LikeState.Like)}
			<Text>{`${totalLike >= 0 ? "+" : ""}${totalLike}`}</Text>
			{createThumb(LikeState.Dislike)}
		</Group>
	);
}
