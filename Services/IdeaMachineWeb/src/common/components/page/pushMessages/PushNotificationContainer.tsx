// Framework
import * as React from "react";
import { connect } from "react-redux";

// Components
import Flex from "common/components/Flex";
import PushNotificationEntry from "./PushNotificationEntry";

// Functionality

// Types
import { PushNotification } from "common/definitions/PushNotificationTypes";
import { ReduxStore } from "common/redux/store";

// Styles
import styles from "./styles/PushNotificationContainer.module.scss";

type ReduxProps = {
	messages: Array<PushNotification>;
};

export const PushNotificationContainer: React.FC<ReduxProps> = ({ messages }) => {
	return (
		<Flex className={styles.Container} direction="Column">
			{messages.map((x) => (
				<PushNotificationEntry notification={x} key={x.timeStamp.toString()} />
			))}
		</Flex>
	);
};

const mapStateToProps = (state: ReduxStore): ReduxProps => ({
	messages: state.pushNotificationReducer.data
});

export default connect(mapStateToProps)(PushNotificationContainer);
