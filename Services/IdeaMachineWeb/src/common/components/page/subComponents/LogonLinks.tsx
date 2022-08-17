// Framework
import * as React from "react";
import { ActionIcon, Button, Text } from "@mantine/core";
import { Login } from "tabler-icons-react";
import useServices from "common/hooks/useServices";

type Props = {
	minified?: boolean;
};

export const LoginLinks = ({ minified = false }: Props) => {
	const { AccountService } = useServices();

	const signIn = React.useCallback(async () => {
		await AccountService.login();
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
