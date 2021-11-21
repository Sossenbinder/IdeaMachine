// Types
import { IdentityErrorCode } from "../types";
import TL from "common/translations/types";

export const getTranslationForErrorCode = (translations: TL, errorCode: IdentityErrorCode) => {

	const translationName = `IdentityError_${IdentityErrorCode[errorCode ?? 0].toString()}`;

	return translations[translationName] as string;
}