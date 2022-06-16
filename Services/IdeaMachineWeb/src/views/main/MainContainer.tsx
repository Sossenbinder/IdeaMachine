// Framework
import * as React from "react";
import { AppShell } from "@mantine/core";

// Components
import NavBar from "common/components/page/NavBar";
import Flex from "common/components/Flex";
import PushNotificationContainer from "common/components/page/pushMessages/PushNotificationContainer";
import Content from "./Content";

// Functionality
import { getTranslations } from "common/translations/translations";

// Styles
import styles from "./styles/MainContainer.module.scss";
import Header from "common/components/page/Header";

export const MainContainer = () => {
	const [loading, setLoading] = React.useState<boolean>(true);

	const data = getTranslations();

	React.useEffect(() => {
		if (data) {
			setLoading(false);
		}
	}, [data]);

	return (
		<Flex className={styles.MainContainer} direction="Column">
			<If condition={loading}>Loading...</If>
			<If condition={!loading}>
				<PushNotificationContainer />
				<AppShell
					header={<Header />}
					styles={{
						root: { height: "100vh", display: "flex", flexDirection: "column" },
						main: { padding: 0 },
						body: { flexGrow: 1 },
					}}
				>
					<Content />
				</AppShell>
			</If>
		</Flex>
	);
};

export default MainContainer;
