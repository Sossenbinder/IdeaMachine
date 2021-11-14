// Framework
import * as moment from "moment";

export function sortByDateAsc<T>(coll: Array<T>, dateSelector: (item: T) => Date): Array<T> {
	return sortByDate(coll, dateSelector, true);
}

export function sortByDateDesc<T>(coll: Array<T>, dateSelector: (item: T) => Date): Array<T> {
	return sortByDate(coll, dateSelector, false);
}

function sortByDate<T>(coll: Array<T>, dateSelector: (item: T) => Date, ascending: boolean): Array<T> {
	if (!coll) {
		return [];
	}

	return coll.slice().sort((x, y) => {
		const xDate = dateSelector(x);
		const yDate = dateSelector(y);

		if (ascending) {
			return moment.utc(xDate).diff(moment.utc(yDate))
		}
		return moment.utc(yDate).diff(moment.utc(xDate))
	});
}