// Framework
import * as React from "react";

// Components
import NavBar from 'common/components/page/NavBar';
import Flex from "common/components/Flex";
import PushNotificationContainer from "common/components/page/pushMessages/PushNotificationContainer";
import Content from "./Content";

// Functionality
import { getTranslations } from 'common/translations/translations';

// Styles
import styles from "./styles/MainContainer.module.less";

export const MainContainer = () => {

	const [loading, setLoading] = React.useState<boolean>(true);

	const data = getTranslations();

	React.useEffect(() => {
		if (data) {
			setLoading(false);
		}
	}, [data]);

	return (
		<Flex
			className={styles.MainContainer}
			direction="Column">
			<If condition={loading}>
				Loading...
			</If>
			<If condition={!loading}>
				<PushNotificationContainer />
				<Flex
					className={styles.MainGrid}
					direction="Column">
					<NavBar />
					<Content />
				</Flex>
			</If>
		</Flex>
	);
}

export default MainContainer;