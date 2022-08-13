import * as React from "react";
import Flex from "common/components/Flex";
import TagDisplay from "../input/TagDisplay";
import UpDownFilter from "./UpDownFilter";
import { IdeaFilterContext } from "../IdeaFilterContext";
import { OrderType } from "modules/ideas/types";
import styles from "./styles/IdeaListFilters.module.scss";
import { Text } from "@mantine/core";

export const IdeaListFilters: React.FC = () => {
	const { filters, updateFilters } = React.useContext(IdeaFilterContext);

	const updateTags = (newTags: Array<string>) => {
		updateFilters({
			...filters,
			tags: newTags,
		});
	};

	return (
		<Flex className={styles.Container} direction="Column" crossAlign="Start">
			<Text className={styles.Heading} mt={5} sx={{ alignSelf: "center" }}>
				Order
			</Text>
			<UpDownFilter type={OrderType.Created} />
			<UpDownFilter type={OrderType.Description} />
			<UpDownFilter type={OrderType.Popularity} />
			<div className={styles.TagDisplay}>
				<TagDisplay tags={filters.tags} setTags={updateTags} />
			</div>
		</Flex>
	);
};

export default IdeaListFilters;
