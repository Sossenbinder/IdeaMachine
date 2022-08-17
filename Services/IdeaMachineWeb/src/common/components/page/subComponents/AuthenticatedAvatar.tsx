import * as React from "react";
import useServices from "common/hooks/useServices";
import useChannel from "common/hooks/useChannel";
import useNotificationBackedCall from "common/hooks/useNotificationBackedCall";
import { ActionIcon, Avatar, Group, Paper, Text } from "@mantine/core";

import styles from "./AuthenticatedAvatar.module.scss";
import { BlackSpinner } from "common/components/controls/Spinner";
import { Link } from "react-router-dom";
import useAccount from "common/hooks/useAccount";
import { Logout } from "tabler-icons-react";
import { useMsal } from "@azure/msal-react";

type Props = {
	showName?: boolean;
};

export const AuthenticatedAvatar = ({ showName = true }: Props) => {
	const account = useAccount();

	const channel = useChannel<FileList>("UpdateProfilePictureTriggered");

	const [running, call] = useNotificationBackedCall("UserDetails");

	const msal = useMsal();

	const onLogoutClick = async () => {
		await msal.instance.logoutRedirect();
	};

	const uploadPicture = React.useCallback(
		(event: React.ChangeEvent<HTMLInputElement>) => {
			const files = event.currentTarget.files;
			if (files.length > 0) {
				call(() => channel.publish(files));
			}
		},
		[call],
	);

	return (
		<Paper
			p="md"
			sx={(theme) => ({
				backgroundColor: theme.colorScheme === "dark" ? theme.black[3] : theme.colors.gray[5],
			})}
		>
			<Group direction="row" position="center" align="center">
				<label htmlFor="PictureUploadHiddenInput">
					<div className={styles.ProfilePictureContainer}>{running ? <BlackSpinner /> : <Avatar src={account.profilePictureUrl} alt="You" />}</div>
				</label>
				<input
					id="PictureUploadHiddenInput"
					className={styles.PictureUploadHiddenInput}
					onChange={running ? void 0 : uploadPicture}
					type="file"
					accept=".png,.img"
				/>
				{showName && (
					<Link to="/account/Overview" className={styles.UserName}>
						<Text size="md">{account.userName}</Text>
					</Link>
				)}
				<ActionIcon>
					<Logout onClick={onLogoutClick} />
				</ActionIcon>
			</Group>
		</Paper>
	);
};
