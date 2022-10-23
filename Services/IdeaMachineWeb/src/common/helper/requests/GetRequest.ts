import { NetworkResponse, Pagination } from "./types/NetworkDefinitions";
import HttpRequest, { RequestMethods } from "./HttpRequest";

export default class GetRequest<TResponse> extends HttpRequest<void, TResponse> {
	constructor(url: string) {
		super(url, RequestMethods.GET);
	}

	public async get(): Promise<NetworkResponse<TResponse>> {
		return super.send();
	}
}

export class PagedGetRequest<TResponse, TTokenType = string> {
	constructor(private readonly url: string, private readonly pageParameterName: string = "page") {}

	public get = async (paginationToken: TTokenType = null): Promise<NetworkResponse<Pagination.PaginationResult<TTokenType, TResponse>>> => {
		const getRequest = new GetRequest<Pagination.PaginationResult<TTokenType, TResponse>>(
			`${this.url}${paginationToken ? `?${this.pageParameterName}=${paginationToken ?? null}` : ""}`,
		);
		return await getRequest.get();
	};

	public async *getContinuous(paginationToken?: TTokenType): AsyncIterableIterator<Array<TResponse>> {
		do {
			const result = await this.get(paginationToken);

			if (!result.success || !!result.payload?.paginationToken) {
				return;
			}

			paginationToken = result.payload.paginationToken;

			yield result.payload.data;
		} while (paginationToken);
	}
}
