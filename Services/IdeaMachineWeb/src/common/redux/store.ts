// Framework
import * as redux from "redux";

// Functionality
import { reducer as ideaReducer } from "modules/ideas/reducer/IdeaReducer";
import { reducer as accountReducer } from "modules/account/reducer/AccountReducer";
import { reducer as paginationReducer, PaginationReducerState } from "./reducer/PaginationReducer";
import { reducer as notificationReducer } from "./reducer/notificationReducer";
import { reducer as pushNotificationReducer } from "./reducer/PushNotificationReducer";

// Types
import { Idea } from "modules/ideas/types";
import { Account } from "modules/account/types";
import { PushNotification } from "common/definitions/PushNotificationTypes";
import { ReducerState, MultiReducerState } from "./reducer/types";
import { Notification } from "common/definitions/NotificationTypes";
import { compose } from "redux";

export type Reducers = {
	accountReducer: ReducerState<Account>;
	pushNotificationReducer: MultiReducerState<PushNotification>;
	notificationReducer: MultiReducerState<Notification>;
	ideaReducer: MultiReducerState<Idea>;
	paginationReducer: PaginationReducerState;
};

export type ReduxStore = redux.Store & Reducers;

const composeEnhancers = (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

export const store: ReduxStore = redux.createStore(
	redux.combineReducers<Reducers>({
		accountReducer: accountReducer.reducer,
		pushNotificationReducer: pushNotificationReducer.reducer,
		notificationReducer: notificationReducer.reducer,
		ideaReducer: ideaReducer.reducer,
		paginationReducer: paginationReducer,
	}),
	composeEnhancers(redux.applyMiddleware()),
);

export default store;
