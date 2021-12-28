// Framework
import * as React from "react";

// Styles
import styles from "./styles/IdeaNotFound.module.scss";

export const IdeaNotFound: React.FC = () => {
	return <span className={styles.IdeaNotFound}>Sadly, no idea could be found with that ID :(</span>;
};

export default IdeaNotFound;
