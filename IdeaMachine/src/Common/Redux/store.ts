// Framework
import * as redux from "redux";

// Functionality
import { reducer as ideaReducer } from "modules/Ideas/Reducer/IdeaReducer";
import { reducer as accountReducer } from "modules/Account/Reducer/AccountReducer";

// Types
import { Idea } from "modules/Ideas/types";
import { Account } from "modules/Account/types";
import { ReducerState, MultiReducerState } from "./Reducer/types";

export type Reducers = {
	accountReducer: ReducerState<Account>;
	ideaReducer: MultiReducerState<Idea>;
}

export type ReduxStore = redux.Store & Reducers

export const store: ReduxStore = redux.createStore(
	redux.combineReducers<Reducers>({
		accountReducer: accountReducer.reducer,
		ideaReducer: ideaReducer.reducer,
	}),
);

export default store;