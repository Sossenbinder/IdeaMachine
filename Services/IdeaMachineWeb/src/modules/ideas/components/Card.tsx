// Framework
import * as React from "react";

// Styles
import styles from "./styles/Card.module.scss";

export const Card: React.FC = ({ children }) => {
	return (
		<div className={styles.CardContainer}>
			<div className={styles.Card}>{children}</div>
		</div>
	);
};

export default Card;
