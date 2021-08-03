// Framework
import * as React from "react";
import { CircularProgress } from "@material-ui/core";

// Functionality
import useAsyncCall from "common/hooks/useAsyncCall";
import useOnceFlag from "common/hooks/useOnceFlag";

// Types
import MaterialIcon, { MaterialIconProps } from "./MaterialIcon";

import styles from "./styles/FeedbackMaterialIcon.module.less";

type FeedbackMaterialIcon = MaterialIconProps<Promise<void>>

export const FeedbackMaterialIcon: React.FC<FeedbackMaterialIcon> = (props) => {

	const feedbackRef = React.useRef<HTMLDivElement>();

	const [loading, call] = useAsyncCall();
	const [ran, markRan, reset] = useOnceFlag();

	const runFadeOut = () => new Promise<void>(resolve => {
		const cb = _ => {
			feedbackRef.current.classList.remove(styles.animateOut);
			feedbackRef.current.onanimationend = undefined;
			resolve();
		};

		feedbackRef.current.onanimationend = cb;
		feedbackRef.current.classList.add(styles.animateOut);
	});

	const onClick = props.onClick === undefined ? undefined : async (event: React.MouseEvent<HTMLSpanElement, MouseEvent>) => {
		await call(() => props.onClick(event));
		await runFadeOut();
		markRan();

		feedbackRef.current.classList.add(styles.animateIn);
		setTimeout(() => reset(), 3000);
	}

	return (
		<>
			<If condition={loading}>
				<CircularProgress
					size={props.size} />
			</If>
			<div
				className={styles.FeedbackMaterialIcon}
				ref={feedbackRef}>
				<If condition={!loading && !ran}>
					<MaterialIcon
						{...props}
						onClick={onClick} />
				</If>
				<If condition={ran}>
					<MaterialIcon
						size={props.size}
						iconName="done" />
				</If>
			</div>
		</>
	);
}