import { NetworkResponse } from "./types/NetworkDefinitions";

export enum RequestMethods {
	GET = "GET",
	POST = "POST",
	DELETE = "DELETE",
}

const tokenHolder = document.getElementsByName("__RequestVerificationToken")[0] as HTMLInputElement;

export default abstract class AjaxRequest<TRequest, TResponse> {

	private m_url: string;

	private m_requestMethod: string;

	constructor(
		url: string,
		requestMethod: RequestMethods = RequestMethods.POST) {
		this.m_url = url;
		this.m_requestMethod = requestMethod;
	}

	protected async send(requestData?: TRequest): Promise<NetworkResponse<TResponse>> {

		const requestInit: RequestInit = {
			method: this.m_requestMethod,
			cache: "no-cache",
			headers: {
				"Accept": "application/json, text/javascript, */*",
				"Content-Type": "application/json",
				"RequestVerificationToken": tokenHolder.value,
			},
			credentials: 'include'
		};

		if ((this.m_requestMethod === RequestMethods.POST || this.m_requestMethod === RequestMethods.DELETE) && typeof requestData !== "undefined") {
			requestInit.body = JSON.stringify(requestData);
		}

		try {
			const response = await fetch(this.m_url, requestInit);
			
			if (response.status === 500) {			
				return {
					success: false,
					payload: undefined,
				};
			}

			const json = await response.json();

			if (json.data === undefined) {
				return {
					success: response.ok,
					payload: undefined,
				};
			}

			return {
				success: response.ok,
				payload: json.data as TResponse,
			};

		} catch (e) {
			return {
				success: false,
				payload: undefined,
			};
		}
	}
}