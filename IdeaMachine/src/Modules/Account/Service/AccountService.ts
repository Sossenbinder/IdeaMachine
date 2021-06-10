// Functionality
import { IAccountService } from "common/Modules/Service/types";
import ModuleService from "common/Modules/Service/ModuleService";
import * as accountCommunication from "modules/Account/Communication/AccountCommunication";
import { NetworkResponse } from "Common/Helper/Requests/Types/NetworkDefinitions";

// Types
import { RegisterInfo, SignInInfo, IdentityErrorCode } from "../types";

export default class AccountService extends ModuleService implements IAccountService {

	public constructor() {
		super();
	}

	public start() {
		return Promise.resolve();
	}

	register = async (registerInfo: RegisterInfo): Promise<NetworkResponse<IdentityErrorCode>> => {
		const result = await accountCommunication.register(registerInfo);
		return result;
	}

	login = async (signInInfo: SignInInfo): Promise<NetworkResponse<IdentityErrorCode>> => {
		await accountCommunication.signIn(signInInfo);

		return null;
	}
}
