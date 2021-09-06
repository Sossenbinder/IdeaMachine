import { ReducerAction } from "./types";

const UPDATE_IDEA_PAGINATION = "UPDATE_IDEA_PAGINATION";

type PaginationToken = number | string | null;

type PaginationInfo = {
	paginationToken: PaginationToken;
}

export type PaginationReducerState = {
	ideaPagination: PaginationInfo;
}

const initialState: PaginationReducerState = {
	ideaPagination: {
		paginationToken: null,
	}
}

export const updateIdeaPagination = (paginationToken: PaginationToken): ReducerAction<PaginationInfo> => {
	return {
		payload: {
			paginationToken,
		},
		type: UPDATE_IDEA_PAGINATION,
	}
}

export const reducer = (state = initialState, action: ReducerAction<PaginationInfo>): PaginationReducerState => {

	const { type, payload } = action;

	switch (type) {
		case UPDATE_IDEA_PAGINATION:
			return {
				...state,
				ideaPagination: payload,
			}
		default:
			return state;
	}
}