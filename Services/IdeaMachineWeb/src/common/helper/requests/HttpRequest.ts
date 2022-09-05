import { NetworkResponse } from "./types/NetworkDefinitions";
import { msalInstance, scopes } from "../../../modules/account/msal/msalConfig";

export enum RequestMethods {
	GET = "GET",
	POST = "POST",
	DELETE = "DELETE",
}

const tokenHolder = document.getElementsByName("__RequestVerificationToken")[0] as HTMLInputElement;

export default abstract class HttpRequest<TRequest, TResponse> {
	private m_url: string;

	private m_requestMethod: string;

	constructor(url: string, requestMethod: RequestMethods = RequestMethods.POST) {
		this.m_url = url;
		this.m_requestMethod = requestMethod;
	}

	protected async send(requestData?: TRequest): Promise<NetworkResponse<TResponse>> {
		const requestOptions: RequestInit = {
			method: this.m_requestMethod,
			cache: "no-cache",
			headers: {
				Accept: "application/json, text/javascript, */*",
				"Content-Type": "application/json",
				RequestVerificationToken: tokenHolder.value,
			},
			credentials: "include",
		};

		if ((this.m_requestMethod === RequestMethods.POST || this.m_requestMethod === RequestMethods.DELETE) && typeof requestData !== "undefined") {
			requestOptions.body = JSON.stringify(requestData);
		}

		const headers = new Headers({
			Accept: "application/json, text/javascript, */*",
			"Content-Type": "application/json",
			RequestVerificationToken: tokenHolder.value,
		});
		const accounts = msalInstance.getAllAccounts();
		if (accounts.length > 0) {
			const tokenInfo = await msalInstance.acquireTokenSilent({
				scopes: [scopes.user],
				account: accounts[0],
			});
			const bearer = `Bearer ${tokenInfo.accessToken}`;
			headers.append("Authorization", bearer);
		}

		requestOptions.headers = headers;

		try {
			const response = await fetch(this.m_url, requestOptions);
			const responseBody = await response.text();

			if (responseBody.length === 0) {
				return {
					success: response.ok,
				};
			}

			const json = JSON.parse(responseBody);

			if (json === undefined) {
				return {
					success: response.ok,
					payload: undefined,
				};
			}

			return {
				success: response.ok,
				payload: json as TResponse,
			};
		} catch (e) {
			return {
				success: false,
				payload: undefined,
			};
		}
	}
}
