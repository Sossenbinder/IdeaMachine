// Framework
import * as React from "react";
import { withRouter } from "react-router";
import { RouteComponentProps } from 'react-router-dom';

// Components
import Flex from "common/components/Flex";
import MaterialIcon from "common/components/MaterialIcon";

// Styles
import styles from "./styles/CreateButton.module.less";

type Props = RouteComponentProps;

export const CreateButton: React.FC<Props> = ({ history }) => {

	return (
		<Flex
			className={styles.CreateButton}
			mainAlign="Center"
			crossAlign="Center"
			onClick={() => history.push("/idea/input")} >
			<MaterialIcon
				iconName="add"
				color="black"
				size={50}
				onClick={() => history.push("/idea/input")} />
		</Flex>
	);
}

export default withRouter(CreateButton);