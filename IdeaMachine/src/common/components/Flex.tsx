// Framework
import * as React from "react";
import classNames from "classnames";

import "./styles/Flex.less";

export enum FlexDirections {
	Column,
	ColumnReverse,
	Row,
	RowReverse
}

enum FlexWrap {
	NoWrap,
	Wrap,
	WrapReverse
}

enum FlexAlign {
	Center,
	Start,
	End
}

enum FlexSpace {
	Around,
	Between
}

export type FlexProps = {
	className?: string;
	style?: React.CSSProperties;
	direction?: keyof typeof FlexDirections;
	wrap?: keyof typeof FlexWrap;
	basis?: string | number;
	grow?: number;
	mainAlign?: keyof typeof FlexAlign;
	mainAlignSelf?: keyof typeof FlexAlign;
	crossAlign?: keyof typeof FlexAlign;
	crossAlignSelf?: keyof typeof FlexAlign;
	space?: keyof typeof FlexSpace;
	children?: React.ReactNode;
	onClick?: () => void;
	onScroll?: (event: React.UIEvent<HTMLDivElement>) => void;
	title?: string;
}

export const Flex = React.forwardRef<HTMLDivElement, FlexProps>((props, ref) => {

	const { className, style, direction = "Row", wrap, mainAlign, mainAlignSelf, crossAlign, crossAlignSelf, space, children, onClick = () => { }, onScroll = () => { }, title = null, basis, grow } = props;

	const flexStyles: React.CSSProperties = {
		...style,
		flexBasis: basis ?? undefined,
		flexGrow: grow ?? undefined,
	};

	const classes = classNames({
		"flex": true,
		"flexColumn": direction === "Column",
		"flexColumnReverse": direction === "ColumnReverse",
		"flexRow": direction === "Row",
		"flexRowReverse": direction === "RowReverse",
		"flexNoWrap": wrap === "NoWrap",
		"flexWrap": wrap === "Wrap",
		"flexWrapReverse": wrap === "WrapReverse",
		"flexMainCenter": mainAlign === "Center",
		"flexMainStart": mainAlign === "Start",
		"flexMainEnd": mainAlign === "End",
		"flexMainCenterSelf": mainAlignSelf === "Center",
		"flexMainStartSelf": mainAlignSelf === "Start",
		"flexMainEndSelf": mainAlignSelf === "End",
		"flexAround": space === "Around",
		"flexBetween": space === "Between",
		"flexCrossCenter": crossAlign === "Center",
		"flexCrossStart": crossAlign === "Start",
		"flexCrossEnd": crossAlign === "End",
		"flexCrossCenterSelf": crossAlignSelf === "Center",
		"flexCrossStartSelf": crossAlignSelf === "Start",
		"flexCrossEndSelf": crossAlignSelf === "End",
	}, className ?? "");

	return (
		<div
			className={classes}
			style={flexStyles}
			onClick={onClick}
			onScroll={onScroll}
			ref={ref}
			title={title}>
			{children}
		</div>
	)
});

export default Flex;