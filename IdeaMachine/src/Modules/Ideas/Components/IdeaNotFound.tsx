// Framework
import * as React from "react";

// Styles
import styles from "./Styles/IdeaNotFound.module.less";

export const IdeaNotFound: React.FC = () => {
	return (
		<span className={styles.IdeaNotFound}>
			Sadly, no idea could be found with that ID :(
		</span>
	);
}

export default IdeaNotFound;