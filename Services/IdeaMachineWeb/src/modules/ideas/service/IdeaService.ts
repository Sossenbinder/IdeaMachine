// Framework
import * as moment from "moment";

// Functionality
import { IIdeaService } from "common/modules/service/types";
import ModuleService from "common/modules/service/ModuleService";
import * as ideaCommunication from "modules/ideas/communication/IdeaCommunication";
import { reducer as ideaReducer } from "modules/ideas/reducer/IdeaReducer";
import { reducer as pushNotificationReducer } from "common/redux/reducer/PushNotificationReducer";
import { updateIdeaPagination } from "common/redux/reducer/PaginationReducer";
import { ensureArray } from "common/helper/arrayUtils";

// Types
import { IChannelProvider } from "common/modules/channel/ChannelProvider";
import { Idea, IdeaDeleteErrorCode, IdeaInputResult } from "../types";
import { CouldBeArray } from "common/types/arrayTypes";

export default class IdeaService extends ModuleService implements IIdeaService {
	public constructor(channelProvider: IChannelProvider) {
		super(channelProvider);
	}

	public async start() {}

	addIdea = async (idea: Idea, attachments?: Array<File>): Promise<number> => {
		let result: IdeaInputResult = null;

		if (!idea.shortDescription) {
			result |= IdeaInputResult.MissingShortDescription;
		}

		if (!idea.longDescription) {
			result |= IdeaInputResult.MissingLongDescription;
		}

		if (result !== null) {
			return result;
		}

		const response = await ideaCommunication.postIdea(idea, attachments);

		if (!response.success) {
			this.dispatch(
				pushNotificationReducer.add({
					message: "An error occured",
					timeStamp: new Date(),
					type: "Error",
					timeout: 5000,
				}),
			);

			return IdeaInputResult.UnspecifiedError;
		}

		return IdeaInputResult.Successful;
	};

	fetchIdeas = async () => {
		const paginationToken = this.getStore().paginationReducer.ideaPagination.paginationToken;

		const ideaResponse = await ideaCommunication.getIdeas(paginationToken as number | null);

		if (ideaResponse.success) {
			const {
				payload: { data, paginationToken },
			} = ideaResponse;

			this.enrichIdeasWithDate(data);

			this.dispatch(updateIdeaPagination(paginationToken));
			this.dispatch(ideaReducer.add(data));
		}
	};

	initializeOwnIdeas = async (): Promise<void> => {
		const ideaResponse = await ideaCommunication.getOwnIdeas();

		if (ideaResponse.success) {
			const { payload } = ideaResponse;

			this.enrichIdeasWithDate(payload);

			this.dispatch(ideaReducer.put(payload));
		}
	};

	getSpecificIdea = async (id: number) => {
		const ideaResponse = await ideaCommunication.getSpecificIdea(id);

		if (ideaResponse.success) {
			this.enrichIdeasWithDate(ideaResponse.payload);
			this.dispatch(ideaReducer.put(ideaResponse.payload));
		}
	};

	deleteIdea = async (id: number) => {
		const deletionResponse = await ideaCommunication.deleteIdea(id);

		const errorCode = deletionResponse.payload;

		if (errorCode === IdeaDeleteErrorCode.Successful) {
			this.dispatch(ideaReducer.delete(this.getStore().ideaReducer.data.find((x) => x.id === id)));
		}

		if (errorCode === IdeaDeleteErrorCode.NotOwned) {
			this.dispatch(
				pushNotificationReducer.add({
					message: "You don't own this idea, so you also can't delete it",
					timeStamp: new Date(),
					type: "Warning",
					timeout: 5000,
				}),
			);
		}
	};

	deleteAttachment = async (ideaId: number, attachmentId: number) => {
		const deletionResponse = await ideaCommunication.deleteAttachment(ideaId, attachmentId);

		if (!deletionResponse.success) {
			this.dispatch(
				pushNotificationReducer.add({
					message: "Error while trying to delete the attachment",
					timeStamp: new Date(),
					type: "Error",
					timeout: 5000,
				}),
			);

			return;
		}

		const idea = this.getStore().ideaReducer.data.find((x) => x.id === ideaId);

		this.dispatch(
			ideaReducer.put({
				...idea,
				attachments: [
					...this.getStore()
						.ideaReducer.data.find((x) => x.id === ideaId)
						.attachments.filter((x) => x.id !== attachmentId),
				],
			}),
		);
	};

	uploadAttachment = async (ideaId: number, file: File) => {
		const uploadResponse = await ideaCommunication.uploadAttachment(ideaId, file);

		if (uploadResponse.success) {
			const idea = this.getStore().ideaReducer.data.find((x) => x.id === ideaId);

			if (!idea) {
				return;
			}

			const newIdea: Idea = {
				...idea,
				attachments: [
					...idea.attachments,
					{
						attachmentUrl: URL.createObjectURL(file),
						id: uploadResponse.payload,
					},
				],
			};

			this.dispatch(ideaReducer.put(newIdea));
		}
	};

	private enrichIdeasWithDate = (data: CouldBeArray<Idea>) => ensureArray(data).forEach((x) => (x.creationDate = moment(x.creationDate).local().toDate()));
}
