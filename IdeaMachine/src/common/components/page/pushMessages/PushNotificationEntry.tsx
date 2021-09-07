// Framework
import * as React from "react";
import { useDispatch } from "react-redux";

// Components
import { Cell, Grid } from "common/components";
import MaterialIcon, { MaterialIconType } from "common/components/MaterialIcon";

// Functionality
import { getUsTime } from "common/utils/timeUtils";
import { reducer as pushNotificationReducer } from "common/redux/reducer/PushNotificationReducer";

// Types
import { PushNotification } from "common/definitions/PushNotificationTypes";

// Styles
import styles from "./styles/PushNotificationEntry.module.less";
import { tickCountDownAwaitable } from "common/helper/asyncUtils";

type Props = {
	notification: PushNotification;
}

export const PushNotificationEntry: React.FC<Props> = ({ notification }) => {

	const notificationRef = React.useRef<HTMLDivElement>(undefined);
	const dispatch = useDispatch();

	const onClose = async () => {
		let opacityDegree = 100;
		await tickCountDownAwaitable(100, 5, () => {
			notificationRef.current.style.opacity = `${opacityDegree}%`;
			opacityDegree--;
			return Promise.resolve();
		});
		dispatch(pushNotificationReducer.delete(notification));
	}

	React.useEffect(() => {
		if (notification.timeout) {
			window.setTimeout(onClose, notification.timeout!);
		}
	}, []);

	let color: string = "white";

	switch (notification.type) {
		case "Error":
			color = "red";
			break;
		case "Warning":
			color = "yellow";
			break;
	}

	return (
		<div
			className={styles.Container}
			ref={notificationRef}
			style={{ backgroundColor: color }}>
			<Grid
				className={styles.Content}
				gridProperties={{
					gridTemplateAreas: `
						'time . close' 
						'body body body'
					`,
					gridTemplateColumns: "2fr 8fr 25px",
					gridTemplateRows: "1fr 1fr",
				}}>
				<Cell
					cellStyles={{
						gridArea: "time",
					}}>
					<span className={styles.TimeStamp}>
						{getUsTime(notification.timeStamp)}
					</span>
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "close",
					}}>
					<MaterialIcon
						className={styles.CloseIcon}
						type={MaterialIconType.Outlined}
						iconName="cancel"
						onClick={onClose} />
				</Cell>
				<Cell
					cellStyles={{
						gridArea: "body",
					}}>
					{notification.message}
				</Cell>
			</Grid>
		</div>
	);
}

export default PushNotificationEntry;