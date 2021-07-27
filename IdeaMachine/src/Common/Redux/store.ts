// Framework
import * as redux from "redux";

// Functionality
import { reducer as ideaReducer } from "modules/Ideas/Reducer/IdeaReducer";
import { reducer as accountReducer } from "modules/Account/Reducer/AccountReducer";
import { reducer as paginationReducer, PaginationReducerState } from "./Reducer/PaginationReducer";
import { reducer as bubbleMessageReducer } from "./Reducer/BubbleMessageReducer";

// Types
import { Idea } from "modules/Ideas/types";
import { Account } from "modules/Account/types";
import { BubbleMessage } from "common/definitions/BubbleMessageTypes";
import { ReducerState, MultiReducerState } from "./Reducer/types";

export type Reducers = {
	accountReducer: ReducerState<Account>;
	bubbleMessageReducer: MultiReducerState<BubbleMessage>;
	ideaReducer: MultiReducerState<Idea>;
	paginationReducer: PaginationReducerState;
}

export type ReduxStore = redux.Store & Reducers

export const store: ReduxStore = redux.createStore(
	redux.combineReducers<Reducers>({
		accountReducer: accountReducer.reducer,
		bubbleMessageReducer: bubbleMessageReducer.reducer,
		ideaReducer: ideaReducer.reducer,
		paginationReducer: paginationReducer,
	}),
);

export default store;