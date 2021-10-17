// Framework
import * as moment from "moment";

export const getUsDate = (date: Date) => {
	const momentizedDate = moment(date);
	return momentizedDate.format("L");
}

export const getUsTime = (date: Date) => {
	const momentizedDate = moment(date);
	return momentizedDate.format("hh:mm:ss A")
}

export const getFormattedTimeDistance = (date: Date) => {
	const momentizedDate = moment(date);
	const diff = moment.duration(momentizedDate.diff(moment()));

	let diffUnit = Math.abs(diff.asMinutes());
	if (diffUnit < 60) {
		return `${diffUnit.toFixed(0)}min`;
	}

	diffUnit = Math.abs(diff.asHours());
	if (diffUnit < 24) {
		return `${diffUnit.toFixed(0)}h`;
	}

	diffUnit = Math.abs(diff.asMonths());

	if (diffUnit < 12) {
		return `${diffUnit.toFixed(0)}m`;
	}

	diffUnit = Math.abs(diff.asYears());

	return `${diffUnit.toFixed(0)}y`;
}