// Framework
import * as React from "react";
import { useParams } from "react-router";
import styled from "@emotion/styled";

// Components
import Flex from "common/components/Flex";
import LinkButton from "common/components/controls/LinkButton";

// Functionality
import useTranslations from "common/hooks/useTranslations";

// Types
import { SocialLoginErrorCodes } from "../../types";

const StyledFlex = styled(Flex)`
	margin: 0 2rem;
	p {
		text-align: center;
	}
	min-height: inherit;
`;

const StyledButtonContainer = styled.div`
	margin: 2rem;
	button {
		width: 100%;
	}
`;

type RouteParams = {
	errorCode?: string;
};

export const SocialLoginErrorView = () => {
	const tl = useTranslations();
	const params = useParams<RouteParams>();

	const codeInfoLookup = React.useMemo(
		() =>
			new Map<SocialLoginErrorCodes, string>([
				[SocialLoginErrorCodes.Unknown, tl.SocialLoginError_Unknown],
				[SocialLoginErrorCodes.InfoUnavailable, tl.SocialLoginError_InfoUnavailable],
				[SocialLoginErrorCodes.EmailNotKnown, tl.SocialLoginError_EmailNotKnown],
				[SocialLoginErrorCodes.CouldntCreateAccount, tl.SocialLoginError_CouldntCreateAccount],
			]),
		[tl],
	);

	let parsedCode: SocialLoginErrorCodes = SocialLoginErrorCodes.Unknown;
	if (params.errorCode !== undefined) {
		parsedCode = Number(params.errorCode);
	}

	return (
		<StyledFlex direction="Column" space="Between">
			<Flex direction="Column" crossAlign="Center">
				<h2>
					<u>{tl.GenericError}</u>
				</h2>
				<p>{tl.SocialLoginErrorHeading}</p>
				<p>{codeInfoLookup.get(parsedCode)}</p>
			</Flex>
			<StyledButtonContainer>
				<LinkButton color="primary" to="/">
					{tl.Home}
				</LinkButton>
			</StyledButtonContainer>
		</StyledFlex>
	);
};

export default SocialLoginErrorView;
