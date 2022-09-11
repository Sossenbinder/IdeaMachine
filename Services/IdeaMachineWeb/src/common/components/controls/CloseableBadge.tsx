import { ActionIcon, Badge, BadgeProps } from "@mantine/core";
import { X } from "tabler-icons-react";
import * as React from "react";

type CloseButtonProps = {
	onDelete(): void;
};

const CloseButton = ({ onDelete }: CloseButtonProps) => (
	<ActionIcon size="xs" variant="transparent" onClick={onDelete}>
		<X />
	</ActionIcon>
);

type Props = BadgeProps<"div"> & {
	children: React.ReactNode;
	onDelete(): void;
};

export default (props: Props) => {
	const { children, onDelete, ...propsWithoutLabel } = props;
	return (
		<Badge {...propsWithoutLabel} rightSection={<CloseButton onDelete={onDelete} />}>
			{children}
		</Badge>
	);
};
