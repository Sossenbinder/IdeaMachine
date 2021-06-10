// Framework
import * as React from "react";
import { Link } from "react-router-dom";
import { Button, FormControlLabel, Switch, TextField } from "@material-ui/core";

// Components
import Flex from "common/Components/Flex";

// Functionality
import useServices from "common/Hooks/useServices";

// Types
import { SignInInfo } from "modules/Account/types";

// Styles
import styles from "./Styles/SignIn.module.less";

export const SignIn: React.FC = () => {

	const { AccountService } = useServices();

	const [signInInfo, setSignInInfo] = React.useState<SignInInfo>({
		email: "",
		password: "",
		rememberMe: false,
	});

	const onSignInClick = async () => {
		await AccountService.login(signInInfo);
	}

	return (
		<Flex
			className={styles.SignInContainer}
			direction="Column"
			crossAlign="Center">
			<h2>Sign in:</h2>
			<TextField
				className={styles.InputField}
				label="Email"
				onChange={e => setSignInInfo({
					...signInInfo,
					email: e.currentTarget.value,
				})}
				value={signInInfo.email}
				variant="outlined" />
			<TextField
				className={styles.InputField}
				label="Password"
				onChange={e => setSignInInfo({
					...signInInfo,
					password: e.currentTarget.value,
				})}
				type="password"
				value={signInInfo.password}
				variant="outlined" />
			<Flex
				className={styles.RememberMeSliderContainer}
				mainAlign="End">
				<FormControlLabel
					control={
						<Switch
							checked={signInInfo.rememberMe}
							onChange={e => setSignInInfo({
								...signInInfo,
								rememberMe: e.currentTarget.checked,
							})}
							color="primary"
						/>
					}
					className={styles.RememberMeSlider}
					label="Remember login?"
				/>
			</Flex>
			<Flex
				className={styles.ActionSection}
				space="Between"
				direction="Row">
				<Link
					className={styles.RegisterLink}
					to="/Logon/Register">
					Create an account instead?
				</Link>
				<Button
					color="primary"
					className={styles.Button}
					onClick={() => onSignInClick()}
					variant="contained">
					Login
				</Button>
			</Flex>
		</Flex>
	);
}

export default SignIn;