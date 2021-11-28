// Functionality
import { IAccountService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as accountCommunication from "modules/account/communication/AccountCommunication";
import { reducer as accountReducer } from "modules/account/reducer/AccountReducer";
import { NetworkResponse } from "common/helper/requests/types/NetworkDefinitions";

// Types
import { IChannelProvider } from "common/modules/channel/ChannelProvider";
import { RegisterInfo, SignInInfo, IdentityErrorCode, Account } from "../types";
import { Notification as ChannelNotification } from "common/modules/channel/types";
import { Notification, Operation } from "common/helper/signalR/types";
import BackendNotification from "common/helper/signalR/Notifications";

export default class AccountService extends ModuleService implements IAccountService {
	public constructor(channelProvider: IChannelProvider) {
		super(channelProvider);
	}

	public async start() {
		this.ChannelProvider.getChannel<FileList>(ChannelNotification.ProfilePictureUpdated).register(this.onProfilePictureUpdated);

		this.ChannelProvider.getBackendChannel<Account>(BackendNotification.UserDetails).register(this.onUserDetailsUpdated);

		const accountRequest = await accountCommunication.getAccount();

		if (accountRequest.success) {
			this.dispatch(accountReducer.replace(accountRequest.payload));
		}
	}

	async onProfilePictureUpdated(fileList: FileList) {
		await accountCommunication.updateProfilePicture(fileList);
	}

	onUserDetailsUpdated = async ({ operation, payload }: Notification<Account>) => {
		switch (operation) {
			case Operation.Update:
				this.dispatch(
					accountReducer.update({
						...this.getStore().accountReducer.data,
						profilePictureUrl: payload.profilePictureUrl
					})
				);
				break;
		}
	};

	logout = async () => {
		const result = await accountCommunication.logout();

		if (result.success) {
			location.href = "/";
			// TODO: Try this once I figured out how to deal with stale antiforgery tokens without reload
			//this.dispatch(accountReducer.delete(this.getStore().accountReducer.data));
		}
	};

	register = async (registerInfo: RegisterInfo): Promise<NetworkResponse<IdentityErrorCode>> => {
		const result = await accountCommunication.register(registerInfo);
		return result;
	};

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
	};

	verifyEmail = async (userName: string, token: string) => {
		const result = await accountCommunication.verifyEmail(userName, token);
		return result;
	};
}
