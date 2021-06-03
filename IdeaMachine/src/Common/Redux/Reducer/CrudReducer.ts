// Functionality
import { ensureArray, removeAt } from "common/Helper/arrayUtils";

// Types
import { CouldBeArray } from "common/Types/arrayTypes";
import { Reducer, ReducerState, MultiReducerState, ReducerAction } from "./types";

type CommonReducerParams = {
	actionIdentifier: string;
}

// No keys needed here. We only have one entry
type SingleReducerParams<TDataType> = CommonReducerParams & {
	additionalActions?: CouldBeArray<ExternalAction<TDataType, ReducerState<TDataType>>>;
}

// Keys are necessary here, so identifying updates for specific items are possible
export type MultiReducerParams<TDataType> = CommonReducerParams & {
	additionalActions?: CouldBeArray<ExternalAction<Array<TDataType>, MultiReducerState<TDataType>>>;
	key: keyof TDataType;
}

type Action<TDataType, TReducerState extends ReducerState<TDataType>> = (state: TReducerState, action: ReducerAction<TDataType>) => TReducerState;

type ExternalAction<TDataType, TReducerState extends ReducerState<TDataType>> = {
	type: string;
	action: Action<TDataType, TReducerState>;
}

type CrudActions<TDataType, TReducerState extends ReducerState<TDataType>> = {
	addAction: Action<TDataType, TReducerState>;
	updateAction: Action<TDataType, TReducerState>;
	deleteAction: Action<TDataType, TReducerState>;
}

export const createSingleReducer = <T>(params: SingleReducerParams<T>) => createReducerInternal<T, ReducerState<T>>(
	{
		...params,
		actions: {
			addAction: (_, action) => {
				return {
					data: { ...action.payload }
				}
			},
			updateAction: (_, action) => {
				return {
					data: { ...action.payload }
				}
			},
			deleteAction: (_, __) => {
				return {
					data: undefined,
				}
			}
		},
		initialState: {
			data: undefined
		},
	}
);

export const createReducer = <T>(params: MultiReducerParams<T>) => createReducerInternal<CouldBeArray<T>, MultiReducerState<T>>(
	{
		...params,
		actions: {
			addAction: (state, action) => {
				const addPayloadAsArray = ensureArray(action.payload);

				return {
					...state,
					data: [...state.data].concat(addPayloadAsArray),
				};
			},
			updateAction: (state, action) => {
				const updatePayloadAsArray = ensureArray(action.payload);
				const updatedData = [...state.data];

				updatePayloadAsArray.forEach(val => {
					const existingItemIndex = updatedData.findIndex(x => x[params.key] === val[params.key]);
					updatedData[existingItemIndex] = val;
				});

				return {
					...state,
					data: updatedData
				};
			},
			deleteAction: (state, action) => {
				const deletePayloadAsArray = ensureArray(action.payload);
				const dataToDelete = [...state.data];

				deletePayloadAsArray.forEach(val => {
					const indexToDelete = dataToDelete.findIndex(x => x[params.key] === val[params.key]);

					if (indexToDelete > -1) {
						removeAt(dataToDelete, indexToDelete);
					}
				});

				return {
					...state,
					data: dataToDelete
				};
			}
		},
		initialState: {
			data: []
		},
	}
);

type GenericReducerParams<TDataType, TReducerState extends ReducerState<TDataType>> = CommonReducerParams & {
	actions: CrudActions<TDataType, TReducerState>;
	externalActions?: Array<ExternalAction<TDataType, TReducerState>>;
	initialState: TReducerState;
}

const createReducerInternal = <TDataType, TReducerState extends ReducerState<TDataType>>(
	params: GenericReducerParams<TDataType, TReducerState>
): Reducer<TDataType, TReducerState> => {

	const { actionIdentifier, externalActions } = params;

	const ADD_IDENTIFIER = `${actionIdentifier}_ADD`;
	const UPDATE_IDENTIFIER = `${actionIdentifier}_UPDATE`;
	const DELETE_IDENTIFIER = `${actionIdentifier}_DELETE`;
	const REPLACE_IDENTIFIER = `${actionIdentifier}_REPLACE`;

	const initialState: TReducerState = params.initialState;

	const { addAction, deleteAction, updateAction } = params.actions;

	const replaceAction = (state: TReducerState, action: ReducerAction<TDataType>): TReducerState => ({ ...state, data: action.payload });

	const reducerActionMap = new Map<string, (state: TReducerState, action: ReducerAction<TDataType>) => TReducerState>([
		[ADD_IDENTIFIER, addAction],
		[UPDATE_IDENTIFIER, updateAction],
		[DELETE_IDENTIFIER, deleteAction],
		[REPLACE_IDENTIFIER, replaceAction]
	]);

	externalActions?.forEach(x => reducerActionMap.set(x.type, x.action));

	const reducer = (state = initialState, action: ReducerAction<TDataType>): TReducerState => {
		const reducerAction = reducerActionMap.get(action.type);

		if (!reducerAction) {
			return state;
		}

		return reducerAction(state, action);
	}

	const actionGenerator = (type: string) => (payload: TDataType): ReducerAction<TDataType> => ({
		type,
		payload,
	});

	return {
		add: actionGenerator(ADD_IDENTIFIER),
		update: actionGenerator(UPDATE_IDENTIFIER),
		delete: actionGenerator(DELETE_IDENTIFIER),
		replace: actionGenerator(REPLACE_IDENTIFIER),
		reducer: reducer,
	}
}