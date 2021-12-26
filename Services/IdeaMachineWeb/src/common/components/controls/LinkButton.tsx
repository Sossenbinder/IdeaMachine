// Framework
import * as React from "react";
import Button from "@mui/material/Button";
import { RouteComponentProps, withRouter } from "react-router-dom";

type Props = RouteComponentProps & {
	color?: "inherit" | "primary" | "secondary" | "success" | "error" | "info" | "warning";
	/**
	 * If `true`, the button will be disabled.
	 */
	disabled?: boolean;
	/**
	 * The size of the button.
	 * `small` is equivalent to the dense button styling.
	 */
	size?: "small" | "medium" | "large";
	/**
	 * The variant to use.
	 */
	variant?: "text" | "outlined" | "contained";
} & {
	to: string;
};

export const LinkButton: React.FC<Props> = ({ to, color, disabled, size = "medium", variant = "contained", children, history }) => {
	const navigate = () => history.push(to);

	return (
		<Button color={"primary"} disabled={disabled} size={size} variant={variant} onClick={navigate}>
			{children}
		</Button>
	);
};

export default withRouter(LinkButton);
