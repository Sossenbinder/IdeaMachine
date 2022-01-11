// Framework
import * as React from "react";
import styled from "@emotion/styled";

// Components
import Flex from "common/components/Flex";
import { BlackSpinner } from "common/components/controls/Spinner";

// Functionality
import useAsyncCall from "common/hooks/useAsyncCall";
import { listAvailableProviders } from "modules/account/communication/SocialLoginCommunication";

const ProviderImage = styled.img`
	height: 2rem;
	width: 2rem;
	cursor: pointer;
	border: 1px solid lightgrey;
	border-radius: 10px;
	padding: 3px;
`;

type Props = {
	rememberMe: boolean;
};

export const SocialSignIn = ({ rememberMe }: Props) => {
	const [loading, call] = useAsyncCall();
	const [providers, setProviders] = React.useState<Array<string>>([]);

	React.useEffect(() => {
		call(async () => {
			const providersResponse = await listAvailableProviders();

			if (providersResponse.success) {
				setProviders(providersResponse.payload);
			}
		});
	}, []);

	return (
		<Flex crossAlign="Center">
			<Choose>
				<When condition={loading}>
					<BlackSpinner />
				</When>
				<Otherwise>
					{providers.map((provider) => (
						<ProviderImage
							src={`/Resources/Icons/${provider}Icon.png`}
							onClick={() => (window.location.href = `/SocialLogin/ExternalLogin?provider=${provider}&rememberMe=${rememberMe}`)}
							key={provider}
						/>
					))}
				</Otherwise>
			</Choose>
		</Flex>
	);
};

export default SocialSignIn;
