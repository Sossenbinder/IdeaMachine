import * as React from "react";
import { Text } from "@mantine/core";

export default (props: { children: React.ReactNode } & React.ComponentProps<typeof Text>) => (
	<Text
		{...props}
		sx={(theme) => ({
			color: theme.colorScheme === "dark" ? "white" : "black",
		})}
	>
		{props.children}
	</Text>
);
