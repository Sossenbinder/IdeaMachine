// Framework
import * as React from "react";

// Components
import { TextFieldProps } from "@material-ui/core";
import StyledTextField from "./StyledTextField";

// Functionality
import useOnceFlag from "common/hooks/useOnceFlag";

type Props = TextFieldProps & {
	validate(str: string): boolean;
};

export const ValidatableTextField: React.FC<Props> = (props) => {

	const [touched, setTouched] = useOnceFlag();

	const [valid, setValid] = React.useState(true);

	const timeoutRef = React.useRef<number>(null);

	React.useEffect(() => {
		if (props.error) {
			setTouched();
			setValid(false);
		}
	}, [props.error]);

	const validate = (value: string) => {
		const isValid = props.validate(value);
		setValid(isValid);
	}

	const onChange = (event) => {
		const value = event.currentTarget.value;
		props.onChange(event);

		if (touched) {
			validate(value);
			return;
		}

		if (timeoutRef.current !== null) {
			clearTimeout(timeoutRef.current);
		}

		timeoutRef.current = window.setTimeout(() => {
			setTouched();
			validate
		}, 500);
	}

	const showError = touched && !valid;

	const { validate: _, ...propsWithoutValidate } = props;

	return (
		<StyledTextField
			{...propsWithoutValidate}
			error={showError}
			helperText={showError ? props.helperText : ""}
			onChange={onChange}
		/>
	);
}

export default ValidatableTextField;