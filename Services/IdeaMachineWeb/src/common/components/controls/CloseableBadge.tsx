import { ActionIcon, Badge, BadgeProps } from "@mantine/core";
import { X } from "tabler-icons-react";
import * as React from "react";

type CloseButtonProps = {
	onDelete(): void;
};

const CloseButton = ({ onDelete }: CloseButtonProps) => (
	<ActionIcon size="xs" color="blue" radius="xl" variant="transparent">
		<X size={10} onClick={onDelete} />
	</ActionIcon>
);

type Props = BadgeProps<"div"> & {
	children: React.ReactNode;
	onDelete(id: string): void;
};

export default (props: Props) => {
	const { children, onDelete, ...propsWithoutLabel } = props;
	return (
		<Badge {...propsWithoutLabel} rightSection={<CloseButton onDelete={() => onDelete(propsWithoutLabel.id)} />}>
			{children}
		</Badge>
	);
};
