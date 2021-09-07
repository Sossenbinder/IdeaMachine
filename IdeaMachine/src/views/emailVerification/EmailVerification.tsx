// Framework
import * as React from "react";
import { useMutation } from "react-query";
import { Link } from "react-router-dom";

// Components
import Flex from "common/components/Flex";
import LoadingButton from "common/components/controls/LoadingButton";

// Functionality
import useServices from "common/hooks/useServices";
import useQueryStringParams from "common/hooks/useQueryStringParams";
import { getTranslationForErrorCode } from "modules/account/helper/IdentityErrorCodeHelper";
import { useTranslations } from "common/hooks/useTranslations";

// Types
import { IdentityErrorCode } from "modules/account/types";

// Styles
import styles from "./styles/EmailVerification.module.less";


export const EmailVerification: React.FC = () => {

	const translations = useTranslations();
	const { AccountService } = useServices();

	const urlParams = useQueryStringParams();
	const userName = urlParams.get("userName");
	const token = urlParams.get("token");

	const verifyMutation = useMutation(async () => {
		const result = await AccountService.verifyEmail(userName, token.replace(/\s/g, '+'));

		if (result.success) {
			return IdentityErrorCode.Success;
		}

		return result.payload;
	});

	return (
		<Flex
			direction="Row"
			mainAlign="Center">
			<Flex
				className={styles.EmailVerificationContainer}
				direction="Column"
				crossAlign="Center">
				<h2>Email Verification:</h2>
				<Choose>
					<When condition={!userName || !token}>
						<p>Sorry, something seems to be broken with the link you used</p>
					</When>
					<When condition={verifyMutation.isSuccess}>
						<Choose>
							<When condition={verifyMutation.data === IdentityErrorCode.Success}>
								<p>Great, you're now verified!</p>
								<Link to="/Logon/login">
									Login now!
								</Link>
							</When>
							<Otherwise>
								<p>Sorry, something failed during verifying.</p>
								<p>{getTranslationForErrorCode(translations, verifyMutation.data)}</p>
							</Otherwise>
						</Choose>
					</When>
					<Otherwise>
						<p>So, you want to verify your account?</p>
						<LoadingButton
							color="primary"
							className={styles.Button}
							onClick={() => {
								verifyMutation.mutate();
								return Promise.resolve();
							}}
							variant="contained">
							Verify
						</LoadingButton>
					</Otherwise>
				</Choose>
			</Flex>
		</Flex>
	);
}

export default EmailVerification;