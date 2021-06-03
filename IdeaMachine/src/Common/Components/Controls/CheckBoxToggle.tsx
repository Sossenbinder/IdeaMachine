// Framework
import * as React from "react";

// Components
import Flex from "common/Components/Flex";

import "./Styles/CheckBoxToggle.less";

type Props = {
	toggleState: boolean;
	onChange: React.Dispatch<boolean>;
}

export const CheckBoxToggle: React.FC<Props> = ({ toggleState, onChange }) => {

	const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		const value = event.target.checked;
		onChange(value)
	};

	return (
		<Flex
			className="CheckBoxToggle"
			direction="Row"
			crossAlign="Center">
			<label className="SwitchContainer">
				<input type="checkbox" checked={toggleState} className="Switch" onChange={handleChange} />
				<span className="SwitchSlider"></span>
			</label>
		</Flex>
	);
}

export default CheckBoxToggle;