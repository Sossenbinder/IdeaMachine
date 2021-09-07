export namespace Pagination {
	export type PaginationResult<TPaginationToken, TDataType> = {
		paginationToken?: TPaginationToken;
		data: Array<TDataType>;
	}
}

export type NetworkResponse<TPayload> = {
	success: boolean;
	payload?: TPayload;
}