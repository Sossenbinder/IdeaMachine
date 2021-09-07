// Framework
import * as React from "react";
import { Link } from "react-router-dom";
import { FormControlLabel, Switch, TextField } from "@material-ui/core";

// Components
import Flex from "common/components/Flex";
import LoadingButton from "common/components/controls/LoadingButton";

// Functionality
import useServices from "common/hooks/useServices";
import { getTranslationForErrorCode } from "modules/account/helper/IdentityErrorCodeHelper";
import useTranslations from "common/hooks/useTranslations";

// Types
import { IdentityErrorCode, SignInInfo } from "modules/account/types";

// Styles
import styles from "./styles/SignIn.module.less";

export const SignIn: React.FC = () => {

	const { AccountService } = useServices();
	const translations = useTranslations();
	const [identityErrorCode, setIdentityErrorCode] = React.useState<IdentityErrorCode>(IdentityErrorCode.Success);

	const [signInInfo, setSignInInfo] = React.useState<SignInInfo>({
		emailUserName: "",
		password: "",
		rememberMe: false,
	});

	const onSignInClick = async () => {
		const resultCode = await AccountService.login(signInInfo);

		if (resultCode !== IdentityErrorCode.Success) {
			setIdentityErrorCode(resultCode);
			return;
		}
	}

	return (
		<Flex
			className={styles.SignInContainer}
			direction="Column"
			crossAlign="Center">
			<h2>Sign in:</h2>
			<TextField
				className={styles.InputField}
				label="Email/Username"
				onChange={e => setSignInInfo({
					...signInInfo,
					emailUserName: e.currentTarget.value,
				})}
				value={signInInfo.emailUserName}
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
			<If condition={identityErrorCode !== IdentityErrorCode.Success}>
				<span className={styles.ErrorDescription}>
					{getTranslationForErrorCode(translations, identityErrorCode)}
				</span>
			</If>
			<Flex
				className={styles.ActionSection}
				space="Between"
				direction="Row">
				<Link
					className={styles.RegisterLink}
					to="/Logon/Register">
					Create an account instead?
				</Link>
				<LoadingButton
					color="primary"
					className={styles.Button}
					onClick={() => onSignInClick()}
					variant="contained">
					Login
				</LoadingButton>
			</Flex>
		</Flex>
	);
}

export default SignIn;