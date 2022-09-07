// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import IdeaListEntry from "./IdeaListEntry";
import LoadingBubbles from "common/components/state/LoadingBubbles";
import IdeaListFilters from "./filter/IdeaListFilters";

// Functionality
import useServices from "common/hooks/useServices";
import useAsyncCall from "common/hooks/useAsyncCall";
import { IdeaFilterContext } from "modules/ideas/components/IdeaFilterContext";

// Types
import { Idea, OrderDirection } from "modules/ideas/types";

// Styles
import styles from "./styles/IdeaList.module.scss";
import { OrderType } from "../types";
import { Group, ScrollArea } from "@mantine/core";

type Props = {
	ideas: Array<Idea>;
};

export const IdeaList: React.FC<Props> = ({ ideas }) => {
	const {
		filters: { order, direction, tags },
	} = React.useContext(IdeaFilterContext);

	const scrollRef = React.createRef<HTMLDivElement>();

	const { IdeaService } = useServices();

	const [moreLoading, call] = useAsyncCall();

	const scrollHandlingInProgress = React.useRef(false);

	const ideasSorted = React.useMemo(() => {
		let newDataSet = [...ideas];

		if (tags && tags.length > 0) {
			newDataSet = newDataSet.filter((x) => tags.every((y) => x.tags.includes(y)));
		}

		let sortCb: (a: Idea, b: Idea) => number;
		switch (order) {
			case OrderType.Created:
				sortCb = (left, right) => {
					if (direction === OrderDirection.Down) {
						return right.creationDate.getTime() - left.creationDate.getTime();
					}
					return left.creationDate.getTime() - right.creationDate.getTime();
				};
				break;
			case OrderType.Description:
				sortCb = (left, right) => {
					if (direction === OrderDirection.Up) {
						return left.shortDescription.localeCompare(right.shortDescription);
					}
					return right.shortDescription.localeCompare(left.shortDescription);
				};
				break;
			case OrderType.Popularity:
				sortCb = (left, right) => {
					if (direction === OrderDirection.Down) {
						return left.ideaReactionMetaData.totalLike - right.ideaReactionMetaData.totalLike;
					}
					return right.ideaReactionMetaData.totalLike - left.ideaReactionMetaData.totalLike;
				};
				break;
		}

		return newDataSet.sort(sortCb);
	}, [ideas, order, direction, tags]);

	const ideasRendered = React.useMemo(() => ideasSorted.map((idea) => <IdeaListEntry idea={idea} key={idea.id} />), [ideasSorted]);

	React.useEffect(() => {
		if (ideas.length === 0) {
			call(IdeaService.fetchIdeas);
		}
	}, []);

	const onScroll = async (ev: Event) => {
		const element = ev.target as any;
		const maxScroll = element.scrollHeight - element.clientHeight;
		const currentScroll = element.scrollTop;

		const yScrollPercentage = currentScroll / maxScroll;

		if (yScrollPercentage < 0.95 || scrollHandlingInProgress.current) {
			return;
		}

		try {
			if (!scrollHandlingInProgress.current) {
				scrollHandlingInProgress.current = true;
				await call(IdeaService.fetchIdeas);
			}

			element.scrollTop = currentScroll;
		} finally {
			scrollHandlingInProgress.current = false;
		}
	};

	React.useEffect(() => {
		if (!scrollRef.current) {
			return;
		}
		scrollRef.current.onscroll = onScroll;
	}, [scrollRef]);

	return (
		<>
			<Flex className={styles.IdeaList} direction="Column" ref={scrollRef}>
				{ideasRendered}
				<If condition={moreLoading}>
					<LoadingBubbles color="white" />
				</If>
			</Flex>
			<IdeaListFilters />
		</>
	);
};

export default IdeaList;
