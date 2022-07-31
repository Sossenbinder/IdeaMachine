import { ActionIcon, Aside, Burger, Button, Group, Text } from "@mantine/core";
import * as React from "react";
import { AuthenticatedAvatar } from "../subComponents/AuthenticatedAvatar";

import { useHistory } from "react-router";
import { Bulb } from "tabler-icons-react";

export const SideMenu = () => {
	const [isOpen, setIsOpen] = React.useState(true);

	const history = useHistory();

	return (
		<Aside
			position={{
				right: 0,
			}}
			width={{ base: isOpen ? 300 : 100 }}
			p="xl"
			sx={(theme) => ({
				backgroundColor: theme.colorScheme === "dark" ? theme.colors.dark[8] : theme.colors.gray[0],
			})}
			styles={(_) => ({
				root: {
					padding: 0,
				},
			})}
		>
			<Group direction="column" align="center" position="apart" noWrap sx={{ height: "100%" }}>
				<Group direction="column" position="center" noWrap sx={{ height: "100%", width: "100%" }}>
					<Burger opened={isOpen} onClick={() => setIsOpen((o) => !o)} title="Menu" sx={{ alignSelf: isOpen ? "end" : "center" }} />
					{isOpen ? (
						<Button leftIcon={<Bulb size={25} />} variant="outline" onClick={() => history.push("/idea/own")}>
							<Text>My ideas</Text>
						</Button>
					) : (
						<ActionIcon onClick={() => history.push("/idea/own")} variant="outline" color="#4dabf7">
							<Bulb size={25} />
						</ActionIcon>
					)}
				</Group>
				<AuthenticatedAvatar showName={isOpen} />
			</Group>
		</Aside>
	);
};

export default SideMenu;
