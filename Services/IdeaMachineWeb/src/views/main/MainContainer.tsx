import * as React from "react";
import { AppShell } from "@mantine/core";
import Flex from "common/components/Flex";
import PushNotificationContainer from "common/components/page/pushMessages/PushNotificationContainer";
import Content from "./Content";
import { getTranslations } from "common/translations/translations";
import styles from "./styles/MainContainer.module.scss";
import SideMenu from "common/components/page/sidemenu/SideMenu";
import Navbar from "../../common/components/page/navbar/Navbar";

const navbarHeightPx = 45;

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
			{loading ? (
				"Loading..."
			) : (
				<>
					<PushNotificationContainer />
					<AppShell
						className={styles.App}
						padding="md"
						aside={<SideMenu />}
						header={<Navbar navbarHeightPx={navbarHeightPx} />}
						styles={{
							main: {
								padding: 0,
							},
							body: {
								height: `calc(100vh - ${navbarHeightPx}px)`,
							},
						}}
					>
						<Content />
					</AppShell>
				</>
			)}
		</Flex>
	);
};

export default MainContainer;
