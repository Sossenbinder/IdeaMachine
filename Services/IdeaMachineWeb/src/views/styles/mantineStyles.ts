import { MantineProviderProps } from "@mantine/styles";

export const customStyles: MantineProviderProps["styles"] = {
	Text: (theme) => ({
		root: {
			color: theme.colorScheme === "dark" ? "white" : "black",
		},
	}),
};
