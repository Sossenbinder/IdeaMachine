// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";

// Functionality
import { range } from "common/helper/arrayUtils";

// Styles
import styles from "./styles/LoadingBubbles.module.less";

type Props = {
	amountOfBubbles?: number;
	color?: string;
}

export const LoadingBubbles: React.FC<Props> = ({ amountOfBubbles = 3, color = "black" }) => {
	const bubbles = React.useMemo(() => {
		return range(amountOfBubbles).map((_, i) => (
			<Flex
				className={styles.LoadingButtonBubble}
				key={`loadingBubble_${i}`}
				style={{
					background: color,
				}}
			/>));
	}, [amountOfBubbles])

	return (
		<Flex className={styles.LoadingButtonBubbleContainer}>
			{bubbles}
		</Flex>
	);
}

export default LoadingBubbles;