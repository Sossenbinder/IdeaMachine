import * as React from "react";
import Flex from "common/components/Flex";
import { Group, TextInput } from "@mantine/core";

import styles from "./styles/TagDisplay.module.scss";
import CloseableBadge from "common/components/controls/CloseableBadge";

type Props = {
	tags: Array<string>;
	setTags: React.Dispatch<Array<string>>;
};

export const TagDisplay: React.FC<Props> = ({ tags, setTags }) => {
	const [currentText, setCurrentText] = React.useState("");

	const onKeyDown = (event: KeyboardEvent) => {
		if (event.key === " " || event.key === "Enter") {
			setTags(tags.concat(currentText));
			setCurrentText("");
		}
	};

	return (
		<Group direction="row">
			<TextInput
				label="Tags"
				value={currentText}
				onKeyDown={(event) => onKeyDown(event as unknown as KeyboardEvent)}
				onChange={(event) => setCurrentText(event.currentTarget.value)}
				sx={{ width: "100px" }}
			/>
			<Flex className={styles.Chips} direction="Row" wrap="Wrap">
				{tags.map((data, index) => (
					<CloseableBadge
						variant="filled"
						color="info"
						id={`Tags_${index}`}
						key={`Tags_${index}`}
						onDelete={(id: string) => {
							const strippedId = id.replace("Tags_", "");

							const newTags = [...tags];
							newTags.splice(+strippedId, 1);
							setTags(newTags);
						}}
					>
						{data}
					</CloseableBadge>
				))}
			</Flex>
		</Group>
	);
};

export default TagDisplay;
