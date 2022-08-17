// Functionality
import { IAccountService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as accountCommunication from "modules/account/communication/AccountCommunication";
import { reducer as accountReducer } from "modules/account/reducer/AccountReducer";

// Types
import { IChannelProvider } from "common/modules/channel/ChannelProvider";
import { Account } from "../types";
import { Notification, Operation } from "common/helper/signalR/types";
import BackendNotification from "common/helper/signalR/Notifications";

export default class AccountService extends ModuleService implements IAccountService {
	public constructor(channelProvider: IChannelProvider) {
		super(channelProvider);
	}

	public async start() {
		this.ChannelProvider.getChannel<FileList>("UpdateProfilePictureTriggered").register(this.onProfilePictureUpdated);
		this.ChannelProvider.getBackendChannel<Account>(BackendNotification.UserDetails).register(this.onUserDetailsUpdated);
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
						profilePictureUrl: `${payload.profilePictureUrl}?${new Date().getTime()}`,
					}),
				);
				break;
		}
	};
}
