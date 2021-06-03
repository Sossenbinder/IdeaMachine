// Framework
import * as React from "react";
import { } from "react";

// Functionality
import classNames from "classnames";

type Props = {
	iconName: string;
	color?: string;
	size?: number;
	onClick?(): void;
	className?: string;
}

export const MaterialIcon: React.FC<Props> = ({ className, iconName, color, size, onClick }) => {

	const classes = classNames({
		"material-icons": true,
	}, className ?? "");

	size ??= 24;
	color ??= "black";

	return (
		<span
			onClick={() => onClick?.()}
			className={classes}
			style={{
				fontSize: size,
				color: color
			}}>
			{iconName}
		</span>
	);
}

export default MaterialIcon;