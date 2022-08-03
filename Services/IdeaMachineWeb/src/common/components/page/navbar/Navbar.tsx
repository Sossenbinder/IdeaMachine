import * as React from "react";
import { Group, Header, Text } from "@mantine/core";
import Logo from "../Logo";
import { useHistory } from "react-router";

import styles from "./Navbar.module.scss";
import LightDarkSwitch from "../../controls/LightDarkSwitch";
import NotificationBell from "../notifications/NotificationBell";

export const Navbar = () => {
	const history = useHistory();

	return (
		<Header
			sx={(theme) => ({
				backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[8] : theme.colors.gray[0],
			})}
			height={45}
			p="xs"
		>
			<Group direction="row" position="apart">
				<Group direction="row" align="center" className={styles.HomeLink} onClick={() => history.push("/")}>
					<Logo size="s" />
					<Text
						sx={(theme) => ({
							color: theme.colorScheme === "dark" ? "white" : "black",
						})}
					>
						IdeaMachine
					</Text>
				</Group>
				<Group direction="row" align="center">
					<NotificationBell />
					<LightDarkSwitch />
				</Group>
			</Group>
		</Header>
	);
};

export default Navbar;
