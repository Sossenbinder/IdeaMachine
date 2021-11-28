// Framework
import * as React from "react";
import { Link } from "react-router-dom";

// Components
import MaterialIcon from "common/components/MaterialIcon";
import { BlackSpinner } from "common/components/controls/Spinner";
import { Flex } from "common/components";

// Functionality
import useServices from "common/hooks/useServices";
import useChannel from "common/hooks/useChannel";

// Types
import { Account } from "modules/account/types";
import useAsyncCall from "common/hooks/useAsyncCall";
import { Notification } from "common/modules/channel/types";

// Styles
import styles from "./styles/NavBarAuthenticated.module.less";

type Props = {
	account: Account;
};

export const NavBarAuthenticated: React.FC<Props> = ({ account }) => {
	const { AccountService } = useServices();

	const channel = useChannel<FileList>(Notification.ProfilePictureUpdated);

	const [running, call] = useAsyncCall();

	const onLogoutClick = async () => {
		await AccountService.logout();
	};

	const uploadPicture = React.useCallback((event: React.ChangeEvent<HTMLInputElement>) => {
		const files = event.currentTarget.files;
		if (files.length > 0) {
			call(() => channel.publish(files));
		}
	}, []);

	return (
		<Flex className={styles.Container} crossAlign="Center" direction="Row">
			<label htmlFor="PictureUploadHiddenInput" className={styles.PictureLabel}>
				<div className={styles.ProfilePictureContainer}>
					<If condition={running}>
						<BlackSpinner />
					</If>
					<If condition={!running}>
						<img className={styles.ProfilePicture} src={account.profilePictureUrl ?? "Resources/Pictures/User/AnonymousUser.png"} />
					</If>
				</div>
			</label>
			<input
				id="PictureUploadHiddenInput"
				className={styles.PictureUploadHiddenInput}
				onChange={running ? void 0 : uploadPicture}
				type="file"
				accept=".png,.img"
			/>
			<Link to="/account/Overview" className={styles.UserName}>
				{account.userName}
			</Link>
			<MaterialIcon color="white" onClick={onLogoutClick} className={styles.Logout} iconName="logout" size={40} />
		</Flex>
	);
};

export default NavBarAuthenticated;
