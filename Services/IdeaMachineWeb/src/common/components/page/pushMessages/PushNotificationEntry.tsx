// Framework
import * as React from "react";
import { useDispatch } from "react-redux";
import { CSSTransition } from "react-transition-group";

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

type Props = {
	notification: PushNotification;
}

export const PushNotificationEntry: React.FC<Props> = ({ notification }) => {

	const notificationRef = React.useRef<HTMLDivElement>(undefined);
	const dispatch = useDispatch();

	const [show, setShow] = React.useState(true);

	const onExit = async () => {
		dispatch(pushNotificationReducer.delete(notification));
	}

	React.useEffect(() => {
		if (notification.timeout) {
			window.setTimeout(() => setShow(false), notification.timeout!);
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
		<div>
			<CSSTransition
				in={show}
				appear
				timeout={5000}
				classNames={{
					appear: styles.ContainerEnter,
					enter: styles.ContainerEnter,
					appearActive: styles.ContainerEnterActive,
					enterActive: styles.ContainerEnterActive,
					exitActive: styles.ContainerExitActive,
					exit: styles.ContainerExit,
				}}
				onExited={onExit}>
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
								type="Outlined"
								iconName="cancel"
								onClick={() => setShow(false)} />
						</Cell>
						<Cell
							cellStyles={{
								gridArea: "body",
							}}>
							{notification.message}
						</Cell>
					</Grid>
				</div>
			</CSSTransition>
		</div>
	);
}

export default PushNotificationEntry;