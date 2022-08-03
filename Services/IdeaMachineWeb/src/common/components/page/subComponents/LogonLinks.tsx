// Framework
import * as React from "react";
import { useHistory } from "react-router-dom";
import { ActionIcon, Button, Text } from "@mantine/core";
import { Key, Login } from "tabler-icons-react";

type Props = {
	minified?: boolean;
};

export const LoginLinks = ({ minified = false }: Props) => {
	const history = useHistory();

	return (
		<>
			{minified ? (
				<>
					<ActionIcon color="primary" onClick={() => history.push("/logon/login")} title="Login" variant="outline">
						<Login />
					</ActionIcon>
					<ActionIcon onClick={() => history.push("/logon/register")} title="Register" variant="outline">
						<Key />
					</ActionIcon>
				</>
			) : (
				<>
					<Button leftIcon={<Login size={25} />} variant="outline" onClick={() => history.push("/logon/login")}>
						<Text
							sx={(theme) => ({
								color: theme.colorScheme === "dark" ? "white" : "black",
							})}
						>
							Login
						</Text>
					</Button>
					<Button leftIcon={<Key size={25} />} variant="outline" onClick={() => history.push("/logon/register")}>
						<Text
							sx={(theme) => ({
								color: theme.colorScheme === "dark" ? "white" : "black",
							})}
						>
							Register
						</Text>
					</Button>
				</>
			)}
		</>
	);
};

export default LoginLinks;
