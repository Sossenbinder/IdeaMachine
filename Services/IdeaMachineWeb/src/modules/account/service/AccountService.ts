// Functionality
import { IAccountService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as accountCommunication from "modules/account/communication/AccountCommunication";
import { reducer as accountReducer } from "modules/account/reducer/AccountReducer";
import { NetworkResponse } from "common/helper/requests/types/NetworkDefinitions";

// Types
import { IChannelProvider } from "common/modules/channel/ChannelProvider";
import { RegisterInfo, SignInInfo, IdentityErrorCode } from "../types";
import { Notification } from "common/modules/channel/types";

export default class AccountService extends ModuleService implements IAccountService {

	public constructor(channelProvider: IChannelProvider) {
		super(channelProvider);
	}

	public async start() {

		this.ChannelProvider
			.getChannel<FileList>(Notification.ProfilePictureUpdated)
			.register(this.onProfilePictureUpdated);
		
		const accountRequest = await accountCommunication.getAccount();

		if (accountRequest.success) {
			this.dispatch(accountReducer.replace(accountRequest.payload));
		}
	}

	async onProfilePictureUpdated(fileList: FileList) {
		await accountCommunication.updateProfilePicture(fileList);
	}

	logout = async () => {
		const result = await accountCommunication.logout();

		if (result.success) {
			location.href = "/";
			// TODO: Try this once I figured out how to deal with stale antiforgery tokens without reload
			//this.dispatch(accountReducer.delete(this.getStore().accountReducer.data));
		}
	}

	register = async (registerInfo: RegisterInfo): Promise<NetworkResponse<IdentityErrorCode>> => {
		const result = await accountCommunication.register(registerInfo);
		return result;
	}

	login = async (signInInfo: SignInInfo): Promise<IdentityErrorCode> => {
		const result = await accountCommunication.signIn(signInInfo);

		if (result.success) {
			const accountRequest = await accountCommunication.getAccount();

			if (accountRequest.success) {
				location.href = "/";
			}

			return IdentityErrorCode.Success;
		}

		return result.payload;
	}

	verifyEmail = async (userName: string, token: string) => {
		const result = await accountCommunication.verifyEmail(userName, token);
		return result;
	}
}
