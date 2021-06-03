// Framework
import * as redux from "redux";

// Functionality
import { reducer as ideaReducer } from "modules/Ideas/Reducer/IdeaReducer";

// Types
import { Idea } from "modules/Ideas/types";
import { MultiReducerState } from "./Reducer/types";

export type Reducers = {
	ideaReducer: MultiReducerState<Idea>;
}

export type ReduxStore = redux.Store & Reducers

export const store: ReduxStore = redux.createStore(
	redux.combineReducers<Reducers>({
		ideaReducer: ideaReducer.reducer,
	}),
);

export default store;