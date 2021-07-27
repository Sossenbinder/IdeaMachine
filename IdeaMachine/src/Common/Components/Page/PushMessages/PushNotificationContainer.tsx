// Framework
import * as React from "react";
import { connect } from "react-redux";

// Components
import Flex from "common/components/Flex";

// Functionality

// Types
import { BubbleMessage } from "common/definitions/BubbleMessageTypes";
import { ReduxStore } from "common/redux/store";

// Styles
import styles from "./styles/PushNotificationContainer.module.less";

type ReduxProps = {
	messages: Array<BubbleMessage>;
}

export const PushNotificationContainer: React.FC<ReduxProps> = ({ messages }) => {

	return (
		<Flex className={styles.Container}>
			Bubble Notification Container
			{messages.map(x => (
				<span>
					Notification
				</span>
			))}
		</Flex>
	);
}

const mapStateToProps = (state: ReduxStore): ReduxProps => ({
	messages: state.bubbleMessageReducer.data,
});

export default connect(mapStateToProps)(PushNotificationContainer);