// Framework
import * as React from "react";
import { Chip } from "@material-ui/core";

// Components
import Flex from "common/components/Flex";
import TagDisplay from "../input/TagDisplay";
import UpDownFilter from "./UpDownFilter";

// Functionality
import { IdeaFilterContext } from "../IdeaFilterContext";

// Types
import { OrderType } from "modules/ideas/types";

// Styles
import styles from "./styles/IdeaListFilters.module.less";

export const IdeaListFilters: React.FC = () => {

	const { filters, updateFilters } = React.useContext(IdeaFilterContext);

	const updateTags = (newTags: Array<string>) => {
		updateFilters({
			...filters,
			tags: newTags,
		});
	}

	return (
		<Flex
			className={styles.Container}
			direction="Column"
			crossAlign="Start">
			<span className={styles.Heading}>
				Order
			</span>
			<UpDownFilter
				type={OrderType.Created} />
			<UpDownFilter
				type={OrderType.Description} />
			<UpDownFilter
				type={OrderType.Popularity} />
			<div className={styles.TagDisplay}>
				<TagDisplay
					tags={filters.tags}
					setTags={updateTags}
				/>
			</div>
		</Flex>
	);
}

export default IdeaListFilters;