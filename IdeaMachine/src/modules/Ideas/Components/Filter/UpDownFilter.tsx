// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import MaterialIcon from "common/components/MaterialIcon";

// Functionality
import { IdeaFilterContext } from "modules/ideas/components/IdeaFilterContext";

// Types
import { OrderType, OrderDirection } from "modules/ideas/types";

import styles from "./styles/UpDownFilter.module.less";

type Props = {
	type: OrderType;
}

export const UpDownFilter: React.FC<Props> = ({ type }) => {

	const { filters, updateFilters } = React.useContext(IdeaFilterContext);

	const [direction, setDirection] = React.useState<OrderDirection>();

	const changeDirection = (dir: OrderDirection) => {
		setDirection(dir);

		updateFilters({
			...filters,
			order: type,
			direction: dir,
		});
	}

	React.useEffect(() => {
		if (filters.order === type) {
			setDirection(filters.direction);
		} else {
			setDirection(null);
		}
	}, [filters.order, filters.direction]);

	return (
		<Flex
			direction="Row"
			className={styles.UpDownFilter}
			crossAlign="Center">
			<Flex direction="Column">
				<MaterialIcon
					iconName="arrow_upward"
					color={direction === OrderDirection.Up ? "blue" : "black"}
					size={20}
					onClick={() => changeDirection(OrderDirection.Up)} />
				<MaterialIcon
					iconName="arrow_downward"
					color={direction === OrderDirection.Down ? "blue" : "black"}
					size={20}
					onClick={() => changeDirection(OrderDirection.Down)} />
			</Flex>
			<span
				className={styles.Label}
				onClick={() => changeDirection(direction === OrderDirection.Up ? OrderDirection.Down : OrderDirection.Up)}>
				{OrderType[type]}
			</span>
		</Flex>
	)
}

export default UpDownFilter;