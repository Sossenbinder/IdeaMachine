// Framework
import * as React from "react";
import ChipInput from "material-ui-chip-input";
import { ClassNameMap } from "@material-ui/core/styles/withStyles";

type Props = {
	customStyleSet?: ClassNameMap<"custom">;
	tags: Array<string>;
	setTags: React.Dispatch<Array<string>>;
}

export const TagDisplay: React.FC<Props> = ({ customStyleSet, tags, setTags }) => {

	return (
		<ChipInput
			className={customStyleSet?.custom ?? ""}
			onAdd={(chip) => setTags([...tags, chip])}
			onDelete={(_, index) => {
				tags.splice(index, 1);
				setTags(tags);
			}}
			label="Tags"
			newChipKeyCodes={[13, 32]}
			value={tags}
			variant="outlined"
		/>
	);
}

export default TagDisplay;