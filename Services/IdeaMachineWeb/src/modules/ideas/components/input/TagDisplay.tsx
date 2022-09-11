import * as React from "react";
import { Group, TextInput } from "@mantine/core";

import styles from "./styles/TagDisplay.module.scss";
import CloseableBadge from "common/components/controls/CloseableBadge";

type Props = {
	tags: Array<string>;
	setTags: React.Dispatch<Array<string>>;
	maximumTags?: number;
};

export const TagDisplay = ({ tags, setTags, maximumTags }: Props) => {
	const [currentText, setCurrentText] = React.useState("");

	const remainingTagsDepleted = tags.length === maximumTags;

	const onKeyDown = (event: KeyboardEvent) => {
		if (event.key === " " || event.key === "Enter") {
			if (tags.length + 1 > maximumTags) {
				return;
			}

			setTags(tags.concat(currentText));
			setCurrentText("");
		}
	};

	const onTagDelete = (index: number) => {
		const newTags = [...tags];
		newTags.splice(index, 1);
		setTags(newTags);
	};

	return (
		<Group direction="row">
			<TextInput
				label="Tags"
				value={currentText}
				placeholder={remainingTagsDepleted ? `Only ${maximumTags} allowed` : "Add tag"}
				onKeyDown={(event) => onKeyDown(event as unknown as KeyboardEvent)}
				onChange={(event) => setCurrentText(event.currentTarget.value)}
				sx={{ width: "125px" }}
				disabled={remainingTagsDepleted}
			/>
			<Group className={styles.Chips} direction="row" spacing="xs">
				{tags.map((data, index) => (
					<CloseableBadge variant="filled" color="info" id={`Tags_${index}`} key={`Tags_${index}`} onDelete={() => onTagDelete(index)}>
						{data}
					</CloseableBadge>
				))}
			</Group>
		</Group>
	);
};

export default TagDisplay;
