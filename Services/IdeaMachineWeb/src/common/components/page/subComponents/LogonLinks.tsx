// Framework
import * as React from "react";
import { RouteComponentProps, useHistory, withRouter } from "react-router-dom";
import { ActionIcon } from "@mantine/core";
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
					<ActionIcon color="primary" onClick={() => history.push("/Logon/login")}>
						<Login />
					</ActionIcon>
					<ActionIcon onClick={() => history.push("/Logon/Register")}>
						<Key />
					</ActionIcon>
				</>
			) : (
				<></>
			)}
		</>
	);
};

export default LoginLinks;
