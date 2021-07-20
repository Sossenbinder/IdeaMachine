// Framework
import * as React from "react";
import { connect } from "react-redux";

// Components
import Flex from "common/Components/Flex";
import IdeaListEntry from "./IdeaListEntry";

// Functionality
import useServices from "common/Hooks/useServices";

// Types
import { ReduxStore } from "common/Redux/store";
import { Idea } from "modules/Ideas/types";

// Styles
import styles from "./Styles/IdeaList.module.less";
import useAsyncCall from "common/Hooks/useAsyncCall";
import LoadingBubbles from "../../../Common/Components/State/LoadingBubbles";

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
				<LoadingBubbles />
			</If>
		</Flex>
	);
}

const mapStateToProps = (state: ReduxStore): Props => ({
	ideas: state.ideaReducer.data,
});

export default connect(mapStateToProps)(IdeaList);