// Functionality
import { IIdeaService } from "common/modules/Service/types";
import ModuleService from "common/modules/Service/ModuleService";
import * as ideaCommunication from "modules/Ideas/Communication/IdeaCommunication";
import { reducer as ideaReducer } from "modules/Ideas/Reducer/IdeaReducer";
import { updateIdeaPagination } from "common/redux/Reducer/PaginationReducer";
import { ensureArray } from "common/helper/arrayUtils";

// Types
import { Idea } from "../types";
import { CouldBeArray } from "common/types/arrayTypes";

export default class IdeaService extends ModuleService implements IIdeaService {

	public constructor() {
		super();
	}

	public start() {
		return Promise.resolve();
	}

	addIdea = async (idea: Idea) => {
		await ideaCommunication.postIdea(idea);
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
		debugger;
	}

	private enrichIdeasWithDate = (data: CouldBeArray<Idea>) => ensureArray(data).forEach(x => x.creationDate = new Date(x.creationDate));
}
