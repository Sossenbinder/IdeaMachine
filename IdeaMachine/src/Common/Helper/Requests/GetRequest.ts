import { NetworkResponse, Pagination } from "./Types/NetworkDefinitions";
import AjaxRequest, { RequestMethods } from "./AjaxRequest"

type VerificationTokenRequest = {
	__RequestVerificationToken?: string;
}

export default class GetRequest<TResponse, TRequest = void> extends AjaxRequest<TRequest, TResponse> {

	constructor(url: string) {
		super(url, RequestMethods.GET);
	}

	public async get(requestData?: TRequest): Promise<NetworkResponse<TResponse>> {

		let getData: TRequest & VerificationTokenRequest = undefined;
		if (requestData) {
			getData = requestData ?? ({} as TRequest & VerificationTokenRequest);
		}

		return super.send(getData);
	}
}