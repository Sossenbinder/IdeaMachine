import { useMantineColorScheme } from "@mantine/styles";
import { ActionIcon, Group } from "@mantine/core";
import * as React from "react";
import { Sun, MoonStars } from "tabler-icons-react";

export default function LightDarkSwitch() {
	const { colorScheme, toggleColorScheme } = useMantineColorScheme();

	return (
		<Group position="apart">
			<ActionIcon variant="default" onClick={() => toggleColorScheme(colorScheme)}>
				{colorScheme === "dark" ? <Sun size={16} /> : <MoonStars size={16} />}
			</ActionIcon>
		</Group>
	);
}
