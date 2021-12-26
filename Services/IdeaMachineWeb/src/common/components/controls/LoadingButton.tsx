// Framework
import * as React from "react";
import Button, { ButtonProps } from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";

type Props = {
	onClick(): Promise<void>;
} & ButtonProps;

export const LoadingButton: React.FC<Props> = (props) => {
	const [loading, setLoading] = React.useState(false);

	const onClickLocal = async () => {
		try {
			setLoading(true);
			await props.onClick();
		} finally {
			setLoading(false);
		}
	};

	return (
		<Button {...props} disabled={loading ? true : props.disabled} onClick={onClickLocal}>
			<Choose>
				<When condition={loading}>
					<CircularProgress size={24} />
				</When>
				<Otherwise>{props.children}</Otherwise>
			</Choose>
		</Button>
	);
};

export default LoadingButton;
