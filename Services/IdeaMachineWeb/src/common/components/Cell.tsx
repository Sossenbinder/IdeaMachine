// Framework
import * as React from "react";

import "./styles/Grid.scss";
import "./styles/Cell.scss";

type CellStyleKeys = "gridColumn" | "gridRow" | "gridArea";

type CellStyles = Pick<React.CSSProperties, CellStyleKeys>;

type CellProps = CellStyles & {
	className?: string;
	gridArea?: string;

	children?: React.ReactNode;
	onClick?: () => void;
	ref?: React.Ref<any>;
};

export const Cell: React.FC<CellProps> = ({ className, gridColumn, gridRow, gridArea, children, onClick, ref }) => {
	const styles: React.CSSProperties = {
		gridColumn,
		gridRow,
		gridArea,
		height: "100%",
		width: "100%"
	};

	return (
		<div className={className} style={styles} onClick={onClick} ref={ref}>
			{children}
		</div>
	);
};

export default Cell;
