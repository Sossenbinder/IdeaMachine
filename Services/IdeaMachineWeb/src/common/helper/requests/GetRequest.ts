import { NetworkResponse } from "./types/NetworkDefinitions";
import HttpRequest, { RequestMethods } from "./HttpRequest"

type VerificationTokenRequest = {
	__RequestVerificationToken?: string;
}

export default class GetRequest<TResponse, TRequest = void> extends HttpRequest<TRequest, TResponse> {

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