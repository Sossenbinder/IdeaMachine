// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import IdeaListEntry from "./IdeaListEntry";

// Functionality
import useServices from "common/hooks/useServices";

// Types
import { Idea } from "modules/Ideas/types";

// Styles
import styles from "./styles/IdeaList.module.less";
import useAsyncCall from "common/hooks/useAsyncCall";
import LoadingBubbles from "../../../common/components/State/LoadingBubbles";

type Props = {
	ideas: Array<Idea>;
}

export const IdeaList: React.FC<Props> = ({ ideas }) => {

	const scrollRef = React.createRef<HTMLDivElement>();

	const { IdeaService } = useServices();

	const [moreLoading, call] = useAsyncCall();

	const scrollHandlingInProcess = React.useRef(false);

	const ideasRendered = React.useMemo(() => ideas
		.sort((x, y) => y.creationDate.getTime() - x.creationDate.getTime())
		.map(idea => (
			<IdeaListEntry
				idea={idea}
				key={idea.id} />
		)), [ideas]);

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
	);
}

export default IdeaList;