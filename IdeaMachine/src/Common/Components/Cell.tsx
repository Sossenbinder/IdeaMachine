// Framework
import * as React from "react";

import "./Styles/Grid.less";
import "./Styles/Cell.less";

type CellStyleKeys = 'gridColumn' | 'gridRow' | 'gridArea';

type CellStyles = Pick<React.CSSProperties, CellStyleKeys>;

type CellProps = {
	className?: string;
	cellStyles?: CellStyles;

	children?: React.ReactNode;
	onClick?: () => void;
	ref?: React.Ref<any>;
}

export const Cell: React.FC<CellProps> = ({
	className,
	cellStyles,
	children,
	onClick,
	ref
}) => {
	return (
		<div
			className={className}
			style={
				{
					...cellStyles,
					height: "100%",
					width: "100%",
				}
			}
			onClick={onClick}
			ref={ref}>
			{children}
		</div>
	)
}

export default Cell;