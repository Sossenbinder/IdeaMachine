// Functionality
import { IIdeaService } from "common/Modules/Service/types";
import ModuleService from "common/Modules/Service/ModuleService";
import * as ideaCommunication from "modules/Ideas/Communication/IdeaCommunication";
import { reducer as ideaReducer } from "modules/Ideas/Reducer/IdeaReducer";

// Types
import { Idea } from "../types";

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

	initializeIdeas = async () => {
		const ideaResponse = await ideaCommunication.getIdeas();

		if (ideaResponse.success) {

			const { payload } = ideaResponse;

			payload.forEach(pl => pl.creationDate = new Date(pl.creationDate));

			this.dispatch(ideaReducer.replace(payload));
		}
	}

	initializeOwnIdeas = async (): Promise<void> => {
		const ideaResponse = await ideaCommunication.getOwnIdeas();

		if (ideaResponse.success) {

			const { payload } = ideaResponse;

			payload.forEach(pl => pl.creationDate = new Date(pl.creationDate));

			this.dispatch(ideaReducer.put(payload));
		}
	}

	getSpecificIdea = async (id: number) => {
		const ideaResponse = await ideaCommunication.getSpecificIdea(id);

		if (ideaResponse.success) {
			this.dispatch(ideaReducer.put(ideaResponse.payload));
		}
	}
}
