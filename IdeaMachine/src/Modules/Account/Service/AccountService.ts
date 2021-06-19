// Functionality
import { IAccountService } from "common/Modules/Service/types";
import ModuleService from "common/Modules/Service/ModuleService";
import * as accountCommunication from "modules/Account/Communication/AccountCommunication";
import { reducer as accountReducer } from "modules/Account/Reducer/AccountReducer";
import { NetworkResponse } from "Common/Helper/Requests/Types/NetworkDefinitions";

// Types
import { RegisterInfo, SignInInfo, IdentityErrorCode } from "../types";

export default class AccountService extends ModuleService implements IAccountService {

	public constructor() {
		super();
	}

	public async start() {
		const accountRequest = await accountCommunication.getAccount();

		if (accountRequest.success) {
			this.dispatch(accountReducer.replace(accountRequest.payload));
		}
	}

	logout = async () => {
		const result = await accountCommunication.logout();

		if (result.success) {
			this.dispatch(accountReducer.delete(this.getStore().accountReducer.data));
		}
	}

	register = async (registerInfo: RegisterInfo): Promise<NetworkResponse<IdentityErrorCode>> => {
		const result = await accountCommunication.register(registerInfo);
		return result;
	}

	login = async (signInInfo: SignInInfo): Promise<IdentityErrorCode> => {
		const result = await accountCommunication.signIn(signInInfo);

		if (result.success) {
			location.href = "/";
		}

		return result.payload;
	}

	verifyEmail = async (userName: string, token: string) => {
		const result = await accountCommunication.verifyEmail(userName, token);
		return result;
	}
}
