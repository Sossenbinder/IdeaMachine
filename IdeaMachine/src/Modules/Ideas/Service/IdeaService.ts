// Framework
import * as moment from "moment";

// Functionality
import { IIdeaService } from "common/modules/Service/types";
import ModuleService from "common/modules/Service/ModuleService";
import * as ideaCommunication from "modules/Ideas/Communication/IdeaCommunication";
import { reducer as ideaReducer } from "modules/Ideas/Reducer/IdeaReducer";
import { reducer as pushNotificationReducer } from "common/redux/Reducer/PushNotificationReducer";
import { updateIdeaPagination } from "common/redux/Reducer/PaginationReducer";
import { ensureArray } from "common/helper/arrayUtils";

// Types
import { Idea, IdeaDeleteErrorCode, IdeaInputResult } from "../types";
import { CouldBeArray } from "common/types/arrayTypes";

export default class IdeaService extends ModuleService implements IIdeaService {

	public constructor() {
		super();
	}

	public start() {
		return Promise.resolve();
	}

	addIdea = async (idea: Idea, attachments?: FileList): Promise<number> => {

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
			this.dispatch(pushNotificationReducer.add({
				message: "An error occured",
				timeStamp: new Date(),
				type: "Error",
				timeout: 5000,
			}));

			return IdeaInputResult.UnspecifiedError;
		}

		return IdeaInputResult.Successful;
	}

	fetchIdeas = async () => {

		const paginationToken = this.getStore().paginationReducer.ideaPagination.paginationToken;

		const ideaResponse = await ideaCommunication.getIdeas(paginationToken as (number | null));

		if (ideaResponse.success) {

			const { payload: { data, paginationToken } } = ideaResponse;

			this.enrichIdeasWithDate(data);

			this.dispatch(updateIdeaPagination(paginationToken));
			this.dispatch(ideaReducer.add(data));
		}
	}

	initializeOwnIdeas = async (): Promise<void> => {
		const ideaResponse = await ideaCommunication.getOwnIdeas();

		if (ideaResponse.success) {

			const { payload } = ideaResponse;

			this.enrichIdeasWithDate(payload);

			this.dispatch(ideaReducer.put(payload));
		}
	}

	getSpecificIdea = async (id: number) => {
		const ideaResponse = await ideaCommunication.getSpecificIdea(id);

		if (ideaResponse.success) {
			this.enrichIdeasWithDate(ideaResponse.payload);
			this.dispatch(ideaReducer.put(ideaResponse.payload));
		}
	}

	deleteIdea = async (id: number) => {
		const deletionResponse = await ideaCommunication.deleteIdea(id);

		const errorCode = deletionResponse.payload;

		if (errorCode === IdeaDeleteErrorCode.Successful) {
			this.dispatch(ideaReducer.delete(this.getStore().ideaReducer.data.find(x => x.id === id)));
		}

		if (errorCode === IdeaDeleteErrorCode.NotOwned) {
			this.dispatch(pushNotificationReducer.add({
				message: "You don't own this idea, so you also can't delete it",
				timeStamp: new Date(),
				type: "Warning",
				timeout: 5000,
			}));
		}
	}

	deleteAttachment = async (ideaId: number, attachmentId: number) => {
		const deletionResponse = await ideaCommunication.deleteAttachment(ideaId, attachmentId);

		if (!deletionResponse.success) {
			this.dispatch(pushNotificationReducer.add({
				message: "Error while trying to delete the attachment",
				timeStamp: new Date(),
				type: "Error",
				timeout: 5000,
			}));
		}
	}

	private enrichIdeasWithDate = (data: CouldBeArray<Idea>) => ensureArray(data).forEach(x => x.creationDate = moment(x.creationDate).local().toDate());
}
