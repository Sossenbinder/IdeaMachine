// Framework
import * as React from "react";
import { Chip } from "@material-ui/core";

// Components
import Flex from "common/components/Flex";
import StyledTextField from "./StyledTextField";

import styles from "./styles/TagDisplay.module.less";

type Props = {
	tags: Array<string>;
	setTags: React.Dispatch<Array<string>>;
}

export const TagDisplay: React.FC<Props> = ({ tags, setTags }) => {

	const [currentText, setCurrentText] = React.useState("");

	const onKeyDown = (event: KeyboardEvent) => {
		if (event.key === " " || event.key === "Enter") {
			setTags(tags.concat(currentText));
			setCurrentText("");
		}
	}

	return (
		<StyledTextField
			InputProps={{
				startAdornment: (
					<Flex
						className={styles.Chips}
						direction="Row"
						wrap="Wrap">
						{tags.map((data, index) => {
							return (
								<Chip
									label={data}
									color="info"
									id={`Tags_${index}`}
									key={`Tags_${index}`}
									onDelete={(x) => {
										const parentId = x.currentTarget.parentElement.id as string;
										const strippedId = parentId.replace("Tags_", "");

										const newTags = [...tags];
										newTags.splice(+strippedId, 1);
										setTags(newTags);
									}}
								/>
							);
						})}
					</Flex>
				),
			}}
			multiline
			rows={1}
			fullWidth
			label="Tags"
			value={currentText}
			onKeyDown={(event) => onKeyDown(event as unknown as KeyboardEvent)}
			onChange={(event) => setCurrentText(event.currentTarget.value)}
			variant="outlined"
		/>
	);
}

export default TagDisplay;