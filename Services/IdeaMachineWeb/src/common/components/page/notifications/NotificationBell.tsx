import * as React from "react";
import { useSelector } from "react-redux";
import { ReduxStore } from "common/redux/store";
import { Notification } from "common/definitions/NotificationTypes";
import { ActionIcon } from "@mantine/core";
import { Bell } from "tabler-icons-react";

type Props = {};

export const NotificationBell = ({}: Props) => {
	const notifications = useSelector<ReduxStore, Array<Notification>>((x) => x.notificationReducer.data);

	return (
		<ActionIcon>
			<Bell />
		</ActionIcon>
	);
};

export default NotificationBell;
