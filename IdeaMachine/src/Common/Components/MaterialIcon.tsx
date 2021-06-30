// Framework
import * as React from "react";
import { } from "react";

// Functionality
import classNames from "classnames";

export enum MaterialIconType {
	Outlined,
	Filled,
	Rounded,
	Sharp,
	TwoTone,
}

type Props = {
	iconName: string;
	color?: string;
	size?: number;
	onClick?(event: React.MouseEvent<HTMLSpanElement, MouseEvent>): void;
	className?: string;
	type?: MaterialIconType;
}

export const MaterialIcon: React.FC<Props> = ({ className, iconName, color, size, onClick, type = MaterialIconType.Filled }) => {

	const classes = classNames({
		"material-icons": type === MaterialIconType.Filled,
		"material-icons-outlined": type === MaterialIconType.Outlined,
		"material-icons-round": type === MaterialIconType.Rounded,
		"material-icons-sharp": type === MaterialIconType.Sharp,
		"material-icons-two-tone": type === MaterialIconType.TwoTone,
	}, className ?? "");

	size ??= 24;
	color ??= "black";

	return (
		<span
			onClick={event => onClick?.(event)}
			className={classes}
			style={{
				fontSize: size,
				color: color,
				userSelect: "none",
			}}>
			{iconName}
		</span>
	);
}

export default MaterialIcon;