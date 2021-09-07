// Framework
import * as React from "react";
import { Button, PropTypes } from "@material-ui/core";
import { RouteComponentProps, withRouter } from "react-router-dom";

type Props = RouteComponentProps & {
	color?: PropTypes.Color;
	/**
	 * If `true`, the button will be disabled.
	 */
	disabled?: boolean;
	/**
	 * The size of the button.
	 * `small` is equivalent to the dense button styling.
	 */
	size?: 'small' | 'medium' | 'large';
	/**
	 * The variant to use.
	 */
	variant?: 'text' | 'outlined' | 'contained';
} & {
	to: string;
}

export const LinkButton: React.FC<Props> = ({ to, color, disabled, size = "medium", variant = "contained", children, history }) => {

	const navigate = () => history.push(to);

	return (
		<Button
			color={"primary"}
			disabled={disabled}
			size={size}
			variant={variant}
			onClick={navigate}>
			{children}
		</Button>
	);
}

export default withRouter(LinkButton);