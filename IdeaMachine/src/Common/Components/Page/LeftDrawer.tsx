// Framework
import * as React from "react";
import { Link } from "react-router-dom";

// Components
import Flex from "common/Components/Flex";
import Divider from 'common/Components/Controls/Divider';

// Functionality

// Types

// Styles
import "./Styles/LeftDrawer.less";

export const LeftDrawer: React.FC = () => {
	return (
		<Flex
			className="LeftDrawer"
			direction="Column">
			<Link
				className="Link"
				to="/compoundinterest">
				<Flex
					className="LinkContainer"
					mainAlign="Center"
					crossAlign="Center">
					Compound interest
				</Flex>
			</Link>
			<Divider />
			<Link
				className="Link"
				to="/compoundinterest">
				<Flex
					className="LinkContainer"
					mainAlign="Center"
					crossAlign="Center">
					Compound interest
				</Flex>
			</Link>
		</Flex>
	);
}

export default LeftDrawer;