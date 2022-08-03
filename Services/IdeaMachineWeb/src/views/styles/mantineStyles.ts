import { MantineProviderProps } from "@mantine/styles";

export const customStyles: MantineProviderProps["styles"] = {
	Text: (_) => ({
		root: {
			color: "black",
		},
	}),
	ActionIcon: (_) => ({
		root: {
			color: "black",
		},
	}),
};
