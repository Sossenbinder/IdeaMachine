import { ActionIcon, Aside, Burger, Button, Group, Text } from "@mantine/core";
import * as React from "react";
import { AuthenticatedAvatar } from "../subComponents/AuthenticatedAvatar";

import { useHistory } from "react-router";
import { Bulb } from "tabler-icons-react";
import useAccount from "common/hooks/useAccount";
import LogonLinks from "common/components/page/subComponents/LogonLinks";

export const SideMenu = () => {
	const [isOpen, setIsOpen] = React.useState(true);

	const history = useHistory();

	const account = useAccount();

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
							<Text
								sx={(theme) => ({
									color: theme.colorScheme === "dark" ? "white" : "black",
								})}
							>
								My ideas
							</Text>
						</Button>
					) : (
						<ActionIcon onClick={() => history.push("/idea/own")} variant="outline" color="#4dabf7">
							<Bulb size={25} />
						</ActionIcon>
					)}
				</Group>
				{!account.isAnonymous ? <AuthenticatedAvatar showName={isOpen} /> : <LogonLinks minified={!isOpen} />}
			</Group>
		</Aside>
	);
};

export default SideMenu;
