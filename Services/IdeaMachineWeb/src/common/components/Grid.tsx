// Framework
import * as React from "react";
import classNames from "classnames";

import "./styles/Grid.scss";

enum GridDisplay {
	Regular,
	Inline
}

type GridStyleKeys =
	| "columnGap"
	| "rowGap"
	| "gridTemplateColumns"
	| "gridAutoColumns"
	| "gridAutoRows"
	| "gridTemplateRows"
	| "justifyContent"
	| "alignContent"
	| "gridTemplateAreas"
	| "gap"
	| "placeItems";

type GridStyles = Pick<React.CSSProperties, GridStyleKeys>;

type GridProps = GridStyles & {
	className?: string;
	display?: GridDisplay;
};

export const Grid: React.FC<GridProps> = ({
	className,
	columnGap,
	rowGap,
	gridTemplateColumns,
	gridAutoColumns,
	gridTemplateRows,
	justifyContent,
	alignContent,
	gridTemplateAreas,
	gap,
	placeItems,
	display = GridDisplay.Regular,
	children
}) => {
	const classes = classNames({
		grid: display === GridDisplay.Regular,
		inlineGrid: display === GridDisplay.Inline
	});

	return (
		<div
			className={`${classes} ${typeof className !== "undefined" ? className : ""}`}
			style={{
				columnGap,
				rowGap,
				gridTemplateColumns,
				gridAutoColumns,
				gridTemplateRows,
				justifyContent,
				alignContent,
				gridTemplateAreas,
				gap,
				placeItems
			}}
		>
			{children}
		</div>
	);
};

export default Grid;
