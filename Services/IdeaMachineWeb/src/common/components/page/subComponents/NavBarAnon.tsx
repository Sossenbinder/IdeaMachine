// Framework
import * as React from "react";
import Button from "@mui/material/Button";
import { RouteComponentProps, withRouter } from "react-router-dom";

export const NavBarAnon: React.FC<RouteComponentProps> = ({ history }) => {
	return (
		<>
			<Button color="primary" onClick={() => history.push("/Logon/login")} variant="contained">
				Login
			</Button>
			<Button onClick={() => history.push("/Logon/Register")} variant="contained">
				Register
			</Button>
		</>
	);
};

export default withRouter(NavBarAnon);
