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
import { IIdeaService } from "common/modules/service/types";

type Props = {
	ideas: Array<Idea>;
	ideaInitializer?: (ideaService: IIdeaService) => Promise<void>;
};

export const IdeaList = ({ ideas, ideaInitializer = (service) => service.fetchIdeas() }: Props) => {
	const {
		filters: { order, direction, tags },
	} = React.useContext(IdeaFilterContext);

	const scrollArea = React.createRef<HTMLDivElement>();

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
			call(() => ideaInitializer(IdeaService));
		}
	}, []);

	const onScroll = async ({ y }: { x: number; y: number }) => {
		const element = scrollArea.current as any;
		const maxScroll = element.scrollHeight - element.clientHeight;

		const yScrollPercentage = y / maxScroll;

		if (yScrollPercentage < 0.95 || scrollHandlingInProgress.current) {
			return;
		}

		try {
			if (!scrollHandlingInProgress.current) {
				scrollHandlingInProgress.current = true;
				await call(IdeaService.fetchIdeas);
			}

			element.scrollTop = y;
		} finally {
			scrollHandlingInProgress.current = false;
		}
	};

	return (
		<ScrollArea className={styles.IdeaListContainer} onScrollPositionChange={onScroll} viewportRef={scrollArea}>
			<Group className={styles.IdeaList} spacing="sm" direction="column">
				{ideasRendered}
				<If condition={moreLoading}>
					<LoadingBubbles color="white" />
				</If>
			</Group>
			<IdeaListFilters />
		</ScrollArea>
	);
};

export default IdeaList;
