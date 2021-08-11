// Framework
import * as React from "react";
import { FormControlLabel, FormLabel, Radio, RadioGroup } from "@material-ui/core";

// Components
import Flex from "common/components/Flex";

// Functionality
import { IdeaFilterContext } from "./IdeaFilterContext";

// Types
import { OrderType } from "modules/Ideas/types";

// Styles
import styles from "./styles/IdeaListFilters.module.less";

export const IdeaListFilters: React.FC = () => {

	const { order, updateOrder } = React.useContext(IdeaFilterContext);

	return (
		<Flex
			className={styles.Container}
			direction="Column"
			crossAlign="Center">
			<FormLabel
				component="legend"
				className={styles.Heading}>
				Order
			</FormLabel>
			<RadioGroup
				name="order"
				value={order}
				onChange={(_, value) => updateOrder(Number(value))}>
				<FormControlLabel
					value={OrderType.Created}
					control={<Radio />}
					color="default"
					label="Created" />
				<FormControlLabel
					value={OrderType.Description}
					control={<Radio />}
					color="default"
					label="Description" />
				<FormControlLabel
					value={OrderType.Popularity}
					control={<Radio />}
					color="default"
					label="Popularity" />
			</RadioGroup>
		</Flex>
	);
}

export default IdeaListFilters;