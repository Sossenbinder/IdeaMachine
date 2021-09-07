// Framework
import * as React from "react";

// Components
import Flex from "common/components/Flex";
import SignInDialog from "modules/account/components/login/SignInDialog";

export const LogonView: React.FC = () => {
	return (
		<Flex
			direction="Column">
			<SignInDialog />
		</Flex>
	);
}

export default LogonView;