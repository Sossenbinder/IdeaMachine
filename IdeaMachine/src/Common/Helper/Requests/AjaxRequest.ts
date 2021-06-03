import { NetworkResponse } from "./Types/NetworkDefinitions";

export enum RequestMethods {
	GET = "GET",
	POST = "POST"
}

const tokenHolder = document.getElementsByName("__RequestVerificationToken")[0] as HTMLInputElement;

export default class AjaxRequest<TRequest, TResponse> {

	private m_url: string;

	private m_requestMethod: string;

	constructor(
		url: string,
		requestMethod: RequestMethods = RequestMethods.POST) {
		this.m_url = url;
		this.m_requestMethod = requestMethod;
	}

	protected async send(requestData?: TRequest, attachVerificationToken: boolean = true): Promise<NetworkResponse<TResponse>> {

		const requestInit: RequestInit = {
			method: this.m_requestMethod,
			cache: "no-cache",
			headers: {
				'Accept': 'application/json, text/javascript, */*',
				'Content-Type': 'application/json'
			},
			credentials: 'include'
		};

		if (attachVerificationToken) {
			requestInit.headers["RequestVerificationToken"] = tokenHolder.value;
		}

		if (this.m_requestMethod === RequestMethods.POST && typeof requestData !== "undefined") {
			requestInit.body = JSON.stringify(requestData);
		}

		try {
			const response = await fetch(this.m_url, requestInit);

			if (!response.ok) {
				return {
					success: false,
					payload: undefined,
				};
			}

			const json = await response.json();

			if (!json.data) {
				return {
					success: false,
					payload: undefined,
				};
			}

			return {
				success: true,
				payload: json.data,
			};

		} catch (e) {
			return {
				success: false,
				payload: undefined,
			};
		}
	}
}