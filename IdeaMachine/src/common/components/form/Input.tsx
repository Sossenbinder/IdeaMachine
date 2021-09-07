// Framework
import * as React from "react";

// Types
import { InputProps } from "./types";

export const Input: React.FC<InputProps> = ({ inputType, onInput, initialValue }) => {

	const [inputValue, setInputValue] = React.useState<string>(initialValue);

	const updateInputValue = (event: React.ChangeEvent<HTMLInputElement>) => {
		const value = event.currentTarget.value;
		setInputValue(value);
		onInput(value);
	}

	return (
		<input
			type={inputType ?? "text"}
			value={initialValue}
			onChange={updateInputValue} />
	);
}

export default Input;