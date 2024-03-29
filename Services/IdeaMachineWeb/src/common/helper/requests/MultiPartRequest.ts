import { NetworkResponse } from "./types/NetworkDefinitions";

const tokenHolder = document.getElementsByName("__RequestVerificationToken")[0] as HTMLInputElement;

type Request<TRequest> = {
	files: FileList | Array<File>;
	data?: TRequest;
};

export default class MultiPartRequest<TRequest, TResponse = void> {
	private m_url: string;

	constructor(url: string) {
		this.m_url = url;
	}

	public async post(requestData?: Request<TRequest>): Promise<NetworkResponse<TResponse>> {
		const formData = new FormData();

		for (const file of requestData.files) {
			formData.append(file.name, file);
		}

		if (requestData.data !== undefined) {
			formData.append("requestData", JSON.stringify(requestData.data));
		}

		const requestInit: RequestInit = {
			method: "POST",
			cache: "no-cache",
			headers: {
				Accept: "application/json, text/javascript, */*",
				RequestVerificationToken: tokenHolder.value,
			},
			credentials: "include",
			body: formData,
		};

		try {
			const response = await fetch(this.m_url, requestInit);
			const responseBody = await response.text();

			if (responseBody.length === 0) {
				return {
					success: response.ok,
				};
			}

			const json = await response.json();

			if (json.data === undefined) {
				return {
					success: false,
					payload: undefined,
				};
			}

			return {
				success: json.success,
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
