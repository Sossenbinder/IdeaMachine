// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import Input from "./Input";

// Types
import { InputProps } from "./types";

type Props = InputProps & {
	labelText: string;
}

export const LabelledInput: React.FC<Props> = (props) => {

	return (
		<Flex direction="Row">
			<label>
				{props.labelText}
				<Input
					{...props} />
			</label>
		</Flex>
	);
}

export default LabelledInput;