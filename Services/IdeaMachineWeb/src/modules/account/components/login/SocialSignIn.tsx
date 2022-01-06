// Framework
import * as React from "react";
import styled from "@emotion/styled";

// Components
import Flex from "common/components/Flex";

type Props = {
	providers: Array<string>;
};

const ProviderImage = styled.img`
	height: 2rem;
	width: 2rem;
	cursor: pointer;
	border: 1px solid lightgrey;
	border-radius: 10px;
	padding: 3px;
`;

export const SocialSignIn = ({ providers }: Props) => {
	return (
		<Flex crossAlign="Center">
			{providers.map((provider) => (
				<ProviderImage
					src={`/Resources/Icons/${provider}Icon.png`}
					onClick={() => (window.location.href = `/SocialLogin/ExternalLogin?provider=${provider}`)}
					key={provider}
				/>
			))}
		</Flex>
	);
};

export default SocialSignIn;
