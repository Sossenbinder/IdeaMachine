import { NetworkResponse } from "./Types/NetworkDefinitions";
import AjaxRequest, { RequestMethods } from "./AjaxRequest"

type VerificationTokenRequest = {
	__RequestVerificationToken?: string;
}

export default class DeleteRequest<TRequest, TResponse = void> extends AjaxRequest<TRequest, TResponse> {

	constructor(url: string) {
		super(url, RequestMethods.DELETE);
	}

	public async delete(requestData?: TRequest): Promise<NetworkResponse<TResponse>> {

		let deleteData: TRequest & VerificationTokenRequest = undefined;
		if (requestData) {
			deleteData = requestData ?? ({} as TRequest & VerificationTokenRequest);
		}

		return super.send(deleteData);
	}
}