// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import SignInDialog from "modules/Account/Components/Login/SignInDialog";

export const LogonView: React.FC = () => {
	return (
		<Flex
			direction="Column">
			<SignInDialog />
		</Flex>
	);
}

export default LogonView;