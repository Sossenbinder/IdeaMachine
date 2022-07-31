import useAccount from "./useAccount";

export const useIsAuthenticatedUser = () => {
	const account = useAccount();

	return !account.isAnonymous;
};
