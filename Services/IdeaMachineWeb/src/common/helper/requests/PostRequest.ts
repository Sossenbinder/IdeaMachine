import HttpRequest, { RequestMethods } from "./HttpRequest"
import { NetworkResponse, Pagination } from "./types/NetworkDefinitions";

type VerificationTokenRequest = {
	__RequestVerificationToken?: string;
}

export default class PostRequest<TRequest, TResponse> extends HttpRequest<TRequest, TResponse> {

	constructor(url: string) {
		super(url, RequestMethods.POST);
	}

	public post(requestData?: TRequest): Promise<NetworkResponse<TResponse>> {

		const postData: TRequest & VerificationTokenRequest = requestData ?? ({} as TRequest & VerificationTokenRequest);

		return super.send(postData);
	}
}

export class PagedPostRequest<TResponse, TTokenType = string> extends HttpRequest<{ paginationToken: TTokenType }, Pagination.PaginationResult<TTokenType, TResponse>> {

	public post = (paginationToken: TTokenType = null) => super.send({
		paginationToken,
	});

	public async *postContinuous(initialPaginationToken?: TTokenType): AsyncIterableIterator<Array<TResponse>> {

		let paginationToken: TTokenType;

		do {
			const result = await super.send({
				paginationToken: initialPaginationToken,
			});

			if (!result.success || !!result.payload?.paginationToken) {
				return;
			}

			paginationToken = result.payload.paginationToken;

			yield result.payload.data;

		} while (!!paginationToken);
	}
}

export class VoidPostRequest<TRequest> extends PostRequest<TRequest, void> { }