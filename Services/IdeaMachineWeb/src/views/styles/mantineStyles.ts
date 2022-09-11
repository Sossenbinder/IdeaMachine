import { MantineProviderProps } from "@mantine/styles";

export const customStyles: MantineProviderProps["styles"] = {
	Text: (theme) => ({
		root: {
			color: "black",
		},
	}),
	ActionIcon: {
		root: {
			color: "black",
		},
	},
};
