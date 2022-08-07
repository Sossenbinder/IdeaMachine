// Framework
import * as React from "react";
import { withRouter } from "react-router";
import { RouteComponentProps } from "react-router-dom";

// Styles
import styles from "./styles/CreateButton.module.scss";
import { Group } from "@mantine/core";
import { Plus } from "tabler-icons-react";

type Props = RouteComponentProps;

export const CreateButton: React.FC<Props> = ({ history }) => {
	return (
		<Group className={styles.CreateButton} position="center" align="center" onClick={() => history.push("/idea/input")}>
			<Plus size={50} color="black" onClick={async () => history.push("/idea/input")} />
		</Group>
	);
};

export default withRouter(CreateButton);
