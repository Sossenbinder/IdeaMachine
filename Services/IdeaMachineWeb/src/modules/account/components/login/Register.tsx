// Framework
import * as React from "react";
import TextField from "@mui/material/TextField";

// Components
import Flex from "common/components/Flex";
import LoadingButton from "common/components/controls/LoadingButton";

// Functionality
import useServices from "common/hooks/useServices";
import { useTranslations } from "common/hooks/useTranslations";
import { getTranslationForErrorCode } from "modules/account/helper/IdentityErrorCodeHelper";

// Types
import { RegisterInfo, IdentityErrorCode } from "modules/account/types";

// Styles
import styles from "./styles/Register.module.scss";
import { Button } from "@mui/material";
import { useHistory } from "react-router";

type PasswordType = keyof Pick<RegisterInfo, "password" | "confirmPassword">;

enum State {
	Register,
	Success,
}

export const Register: React.FC = () => {
	const { AccountService } = useServices();
	const translations = useTranslations();

	const history = useHistory();

	const [state, setState] = React.useState<State>(State.Register);
	const [identityErrorCode, setIdentityErrorCode] = React.useState<IdentityErrorCode>(IdentityErrorCode.Success);

	const [registerInfo, setRegisterInfo] = React.useState<RegisterInfo>({
		email: "",
		userName: "",
		password: "",
		confirmPassword: "",
	});

	const [incompatiblePassword, setIncompatiblePassword] = React.useState(false);

	const onRegisterClick = async () => {
		if (!incompatiblePassword) {
			const result = await AccountService.register(registerInfo);

			if (result.success) {
				setState(State.Success);
			} else {
				setIdentityErrorCode(result.payload);
			}
		}
	};

	const onPasswordChange = (passwordType: PasswordType, value: string) => {
		const info = { ...registerInfo };

		info[passwordType] = value;

		setRegisterInfo(info);
		setIncompatiblePassword(info.password !== info.confirmPassword);
	};

	return (
		<Flex className={styles.RegisterContainer} direction="Column" crossAlign="Center">
			<span>Register:</span>
			<Choose>
				<When condition={state === State.Register}>
					<TextField
						className={styles.InputField}
						label="Email"
						onChange={(e) =>
							setRegisterInfo({
								...registerInfo,
								email: e.currentTarget.value,
							})
						}
						value={registerInfo.email}
						variant="outlined"
					/>
					<TextField
						className={styles.InputField}
						label="Username"
						onChange={(e) =>
							setRegisterInfo({
								...registerInfo,
								userName: e.currentTarget.value,
							})
						}
						value={registerInfo.userName}
						variant="outlined"
					/>
					<TextField
						className={styles.InputField}
						error={incompatiblePassword}
						label="Password"
						onChange={(e) => onPasswordChange("password", e.currentTarget.value)}
						type="password"
						value={registerInfo.password}
						variant="outlined"
					/>
					<TextField
						className={styles.InputField}
						error={incompatiblePassword}
						label="Confirm password"
						onChange={(e) => onPasswordChange("confirmPassword", e.currentTarget.value)}
						type="password"
						value={registerInfo.confirmPassword}
						variant="outlined"
					/>
					<If condition={identityErrorCode !== IdentityErrorCode.Success}>
						<span className={styles.ErrorDescription}>{getTranslationForErrorCode(translations, identityErrorCode)}</span>
					</If>
					<Flex className={styles.ActionSection} crossAlignSelf="End">
						<Button color="secondary" className={styles.Button} variant="contained" onClick={() => history.push("/Logon/Login")}>
							Login
						</Button>
						<LoadingButton
							color="primary"
							className={styles.Button}
							disabled={incompatiblePassword}
							onClick={() => onRegisterClick()}
							variant="contained"
						>
							Register
						</LoadingButton>
					</Flex>
				</When>
				<Otherwise>Successfully registered. Please check your mails for the confirmation mail!</Otherwise>
			</Choose>
		</Flex>
	);
};

export default Register;
