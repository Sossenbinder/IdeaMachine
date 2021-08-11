// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import IdeaListEntry from "./IdeaListEntry";
import LoadingBubbles from "../../../common/components/State/LoadingBubbles";
import IdeaListFilters from "./IdeaListFilters";

// Functionality
import useServices from "common/hooks/useServices";
import useAsyncCall from "common/hooks/useAsyncCall";
import { IdeaFilterContext } from "modules/Ideas/Components/IdeaFilterContext";

// Types
import { Idea } from "modules/Ideas/types";

// Styles
import styles from "./styles/IdeaList.module.less";
import { OrderType } from "../types";

type Props = {
	ideas: Array<Idea>;
}

export const IdeaList: React.FC<Props> = ({ ideas }) => {

	const { order } = React.useContext(IdeaFilterContext);

	const scrollRef = React.createRef<HTMLDivElement>();

	const { IdeaService } = useServices();

	const [moreLoading, call] = useAsyncCall();

	const scrollHandlingInProcess = React.useRef(false);

	const ideasSorted = React.useMemo(() => {
		let newDataSet = [...ideas];

		let sortCb: (a: Idea, b: Idea) => number;
		switch (order) {
			case OrderType.Created:
				sortCb = (left, right) => left.creationDate.getTime() - right.creationDate.getTime();
				break;
			case OrderType.Description:
				sortCb = (left, right) => left.shortDescription.localeCompare(right.shortDescription);
				break;
			case OrderType.Popularity:
				sortCb = (left, right) => right.ideaReactionMetaData.totalLike - left.ideaReactionMetaData.totalLike;
				break;
		}

		return newDataSet.sort(sortCb);
	}, [ideas, order]);

	const ideasRendered = React.useMemo(() => ideasSorted
		.map(idea => (
			<IdeaListEntry
				idea={idea}
				key={idea.id} />
		)), [ideasSorted]);

	React.useEffect(() => {
		call(IdeaService.fetchIdeas);
	}, []);

	const onScroll = async (ev: Event) => {
		const element = (ev.target) as any;
		const maxScroll = element.scrollHeight - element.clientHeight;
		const currentScroll = element.scrollTop;

		const yScrollPercentage = currentScroll / maxScroll;

		if (yScrollPercentage < 0.95 || scrollHandlingInProcess.current) {
			return;
		}

		try {
			scrollHandlingInProcess.current = true;

			await call(IdeaService.fetchIdeas);

			element.scrollTop = currentScroll;

		} finally {
			scrollHandlingInProcess.current = false;
		}
	}

	React.useEffect(() => {
		if (!scrollRef.current) {
			return;
		}
		scrollRef.current.onscroll = onScroll;
	}, [scrollRef]);

	return (
		<>
			<Flex
				className={styles.IdeaList}
				direction="Column"
				ref={scrollRef}>
				{ideasRendered}
				<If condition={moreLoading}>
					<LoadingBubbles
						color="white" />
				</If>
			</Flex>
			<IdeaListFilters />
		</>
	);
}

export default IdeaList;