import AjaxRequest, { RequestMethods } from "./AjaxRequest"
import { NetworkResponse, Continuation } from "./Types/NetworkDefinitions";

type VerificationTokenRequest = {
	__RequestVerificationToken?: string;
}

export default class PostRequest<TRequest, TResponse> extends AjaxRequest<TRequest, TResponse> {

	constructor(url: string) {
		super(url, RequestMethods.POST);
	}

	public async post(requestData?: TRequest, attachVerificationToken: boolean = true): Promise<NetworkResponse<TResponse>> {

		const postData: TRequest & VerificationTokenRequest = requestData ?? ({} as TRequest & VerificationTokenRequest);

		return super.send(postData, attachVerificationToken);
	}
}

export class ContinuationRequest<TRequest, TResponse> extends PostRequest<TRequest, Continuation.ContinuationResult<TResponse>> {

	async *postContinuous(requestData?: TRequest, attachVerificationToken: boolean = true): AsyncIterableIterator<Array<TResponse>> {

		let continuationToken: string;

		do {
			const postData: Continuation.ContinuationRequest<TRequest> = {
				...requestData,
				continuationToken
			};

			const result = await super.post(postData, attachVerificationToken);

			if (!result.success || !!result.payload?.continuationToken) {
				return;
			}

			continuationToken = result.payload.continuationToken;

			yield result.payload.payload;

		} while (!!continuationToken);
	}
}

export class VoidPostRequest<TRequest> extends PostRequest<TRequest, void> { }