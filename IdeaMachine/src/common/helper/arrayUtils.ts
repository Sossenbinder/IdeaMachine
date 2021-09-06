import { CouldBeArray } from "common/types/arrayTypes";

export const ensureArray = <T>(potentialArray: CouldBeArray<T>) => {
	if (potentialArray instanceof Array) {
		return potentialArray;
	}

	return [potentialArray];
}

export const removeAt = <T>(arr: Array<T>, index: number) => {
	arr.splice(index, 1);
}

export const range = (count: number) => {
	return [...Array(count).keys()];
}