// Framework
import * as React from "react";

// Components
import Flex from "common/Components/Flex";

// Functionality
import { range } from "common/Helper/arrayUtils";

// Styles
import styles from "./Styles/LoadingBubbles.module.less";

type Props = {
	amountOfBubbles?: number;
}

export const LoadingBubbles: React.FC<Props> = ({ amountOfBubbles = 3 }) => {

	const bubbles = React.useMemo(() => {
		return range(amountOfBubbles).map((_, i) => (
			<Flex
				className={styles.LoadingButtonBubble}
				key={`loadingBubble_${i}`}
			/>));
	}, [amountOfBubbles])

	return (
		<Flex className={styles.LoadingButtonBubbleContainer}>
			{bubbles}
		</Flex>
	);
}

export default LoadingBubbles;