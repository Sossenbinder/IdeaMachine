import * as React from "react";
import { AppShell } from "@mantine/core";
import Flex from "common/components/Flex";
import PushNotificationContainer from "common/components/page/pushMessages/PushNotificationContainer";
import Content from "./Content";
import { getTranslations } from "common/translations/translations";
import styles from "./styles/MainContainer.module.scss";
import SideMenu from "common/components/page/sidemenu/SideMenu";
import Navbar from "../../common/components/page/navbar/Navbar";
import { useAccount, useIsAuthenticated, useMsal } from "@azure/msal-react";
import { loginRequest, scopes } from "../../modules/account/msal/msalConfig";

export const MainContainer = () => {
	const [loading, setLoading] = React.useState<boolean>(true);

	const data = getTranslations();

	const msal = useMsal();

	const account = useAccount(msal.accounts[0] || {});
	React.useEffect(() => {
		if (msal.accounts.length === 0) {
			console.log("No accounts");
			return;
		}
		msal.instance
			.acquireTokenSilent({
				scopes: [scopes.user],
				account: account,
			})
			.then((res) => console.log(res));
	}, []);

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
						padding="md"
						aside={<SideMenu />}
						header={<Navbar />}
						styles={{
							main: {
								padding: 0,
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
