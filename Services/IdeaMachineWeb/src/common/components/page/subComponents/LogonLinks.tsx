// Framework
import * as React from "react";
import { useHistory } from "react-router-dom";
import { ActionIcon, Button, Text } from "@mantine/core";
import { Login } from "tabler-icons-react";
import { useMsal } from "@azure/msal-react";
import { loginRequest } from "../../../../modules/account/msal/msalConfig";

type Props = {
	minified?: boolean;
};

export const LoginLinks = ({ minified = false }: Props) => {
	const msal = useMsal();

	const signIn = React.useCallback(() => {
		msal.instance.loginRedirect(loginRequest);
	}, []);

	return (
		<>
			{minified ? (
				<>
					<ActionIcon color="primary" onClick={signIn} title="Sign in" variant="outline">
						<Login />
					</ActionIcon>
				</>
			) : (
				<>
					<Button leftIcon={<Login size={25} />} variant="outline" onClick={signIn}>
						<Text
							sx={(theme) => ({
								color: theme.colorScheme === "dark" ? "white" : "black",
							})}
						>
							Sign in
						</Text>
					</Button>
				</>
			)}
		</>
	);
};

export default LoginLinks;
