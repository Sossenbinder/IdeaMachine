export namespace Continuation {
	export type ContinuationRequest<TRequest> = TRequest & {
		continuationToken: string;
	}

	export type ContinuationResult<TPayload> = {
		continuationToken: string;
		payload: Array<TPayload>;
	}
}

export type NetworkResponse<TPayload> = {
	success: boolean;
	payload?: TPayload;
}