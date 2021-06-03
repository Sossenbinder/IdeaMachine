// Framework
import * as React from "react";

// Components
import Flex from "common/Components/Flex";

// Functionality

// Types

// Styles
import styles from "./Styles/Spacing.module.less";

type Props = {
	x?: number;
	y?: number;
}

export const Spacing: React.FC<Props> = ({ x, y }) => {
	return (
		<div style={{
			height: y !== undefined ? `${y}px` : "100%",
			width: x !== undefined ? `${x}px` : "100%",
		}} />
	);
}

export default Spacing;