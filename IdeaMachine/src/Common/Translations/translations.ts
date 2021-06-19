// Framework
import { useQuery } from "react-query";

// Types
import TL from "./types";

const supportedTranslations = ["en", "de"];

const tlQueryClient = "translations_queryClient";

export const getTranslations = () => {
	const { data } = useQuery(tlQueryClient, async () => {
		let language = navigator.language;

		const dashIndex = language.indexOf("-");
		if (dashIndex !== -1) {
			language = language.substring(0, dashIndex - 1);
		}

		if (supportedTranslations.indexOf(language) === -1) {
			language = "en";
		}

		const tlFile = await fetch(`/Translations/tl_${language}.json`);

		const json = await tlFile.json();

		return json as TL;
	}, {
		staleTime: 10000000,
	});

	return data;
}