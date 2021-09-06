// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";

// Styles
import "./styles/LoadingBar.less";

type Props = {
	progress: number;
}

export const LoadingBar: React.FC<Props> = ({ progress }) => {
	return (
		<div className={"LoadingBar"}>
			<div
				className="Bar"
				style={{
					background: `linear-gradient(to right, grey, grey ${progress}%, transparent 1px, transparent 100%)`
				}}>
				<Flex
					className="ProgressLabel"
					crossAlign="Center"
					mainAlign="Center">
					{`${progress.toFixed(0)}%`}
				</Flex>
			</div>
		</div>
	);
}

export default LoadingBar;