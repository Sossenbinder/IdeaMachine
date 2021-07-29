// Functionality
import { ensureArray, removeAt } from "common/helper/arrayUtils";

// Types
import { CouldBeArray } from "common/types/arrayTypes";
import { Reducer, ReducerState, MultiReducerState, ReducerAction } from "./types";

type CommonReducerParams = {
	actionIdentifier: string;
}

// No keys needed here. We only have one entry
type SingleReducerParams<TDataType> = CommonReducerParams & {
	additionalActions?: CouldBeArray<ExternalAction<TDataType, ReducerState<TDataType>>>;
	initialState?: TDataType;
}

// Keys are necessary here, so identifying updates for specific items are possible
export type MultiReducerParams<TDataType> = CommonReducerParams & {
	additionalActions?: CouldBeArray<ExternalAction<Array<TDataType>, MultiReducerState<TDataType>>>;
	key: keyof TDataType;
	initialState?: Array<TDataType>;
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
	putAction: Action<TDataType, TReducerState>;
}

export const createSingleReducer = <T>(params: SingleReducerParams<T>) => createReducerInternal<T, ReducerState<T>>(
	{
		...params,
		actions: {
			addAction: (_, action) => {
				return {
					data: { ...action.payload },
				}
			},
			updateAction: (_, action) => {
				return {
					data: { ...action.payload },
				}
			},
			deleteAction: (_, __) => {
				return {
					data: undefined,
				}
			},
			putAction: (_, action) => {
				return {
					data: { ...action.payload },
				}
			}
		},
		initialState: {
			data: params.initialState ?? undefined,
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
					const index = updatedData.findIndex(x => x[params.key] === val[params.key]);

					if (index !== -1) {
						updatedData[index] = val;
					}
				});

				return {
					...state,
					data: updatedData,
				};
			},
			deleteAction: (state, action) => {
				const deletePayloadAsArray = ensureArray(action.payload);
				const dataToDelete = [...state.data];

				deletePayloadAsArray.forEach(val => {
					const index = dataToDelete.findIndex(x => x[params.key] === val[params.key]);

					if (index > -1) {
						removeAt(dataToDelete, index);
					}
				});

				return {
					...state,
					data: dataToDelete,
				};
			},
			putAction: (state, action) => {
				const putPayloadAsArray = ensureArray(action.payload);
				const dataCopy = [...state.data];

				putPayloadAsArray.forEach(val => {
					const existingIndex = dataCopy.findIndex(x => x[params.key] === val[params.key]);

					if (existingIndex === -1) {
						dataCopy.push(val);
					} else {
						dataCopy[existingIndex] = val;
					}
				})

				return {
					...state,
					data: dataCopy,
				};
			}
		},
		initialState: {
			data: params.initialState ?? [],
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
	const PUT_IDENTIFIER = `${actionIdentifier}_PUT`;
	const DELETE_IDENTIFIER = `${actionIdentifier}_DELETE`;
	const REPLACE_IDENTIFIER = `${actionIdentifier}_REPLACE`;

	const initialState: TReducerState = params.initialState;

	const { addAction, deleteAction, updateAction, putAction } = params.actions;

	const replaceAction = (state: TReducerState, action: ReducerAction<TDataType>): TReducerState => ({ ...state, data: action.payload });

	const reducerActionMap = new Map<string, (state: TReducerState, action: ReducerAction<TDataType>) => TReducerState>([
		[ADD_IDENTIFIER, addAction],
		[UPDATE_IDENTIFIER, updateAction],
		[DELETE_IDENTIFIER, deleteAction],
		[PUT_IDENTIFIER, putAction],
		[REPLACE_IDENTIFIER, replaceAction]
	]);

	externalActions?.forEach(x => reducerActionMap.set(x.type, x.action));

	const reducer = (state = initialState, action: ReducerAction<TDataType>): TReducerState => {
		const reducerAction = reducerActionMap.get(action.type);
		return reducerAction?.(state, action) ?? state;
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
		put: actionGenerator(PUT_IDENTIFIER),
		reducer: reducer,
	}
}