// Framework
import * as React from "react";
import classNames from "classnames";

// Styles
import styles from "./styles/Separator.module.scss";
import Flex from "common/components/Flex";

enum Direction {
	Horizontal,
	Vertical
}

type Props = {
	className?: string;
	direction?: keyof typeof Direction;
	height?: string;
	width?: string;
};

export const Separator: React.FC<Props> = ({ className, direction = "Horizontal", height = "100%", width = "100%" }) => {
	const cn = classNames(styles.Separator, className, {
		[styles.Horizontal]: direction === "Horizontal",
		[styles.Vertical]: direction === "Vertical"
	});

	return (
		<Flex
			style={{
				width,
				height
			}}
			crossAlign="Center"
			mainAlign="Center"
		>
			<span
				className={cn}
				style={{
					borderRight: direction === "Vertical" ? "1px solid white" : null,
					borderBottom: direction === "Horizontal" ? "1px solid white" : null
				}}
			/>
		</Flex>
	);
};

export default Separator;
