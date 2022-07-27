// Framework
import * as React from "react";
import { AppShell, Navbar } from "@mantine/core";

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
				{/* <AppShell
					header={<Header />}
					styles={{
						root: { height: "100vh", display: "flex", flexDirection: "column" },
						main: { padding: 0 },
						body: { flexGrow: 1 },
					}}
				>
					<Content />
				</AppShell> */}
				<AppShell
					padding="md"
					navbar={
						<Navbar width={{ base: 300 }} height={500} p="xs">
							{/* Navbar content */}
						</Navbar>
					}
					//header={<Header height={60} p="xs">{/* Header content */}</Header>}
					styles={(theme) => ({
						main: { backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[8] : theme.colors.gray[0] },
					})}
				>
					<Content />
				</AppShell>
			</If>
		</Flex>
	);
};

export default MainContainer;
