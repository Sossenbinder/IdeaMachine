// Framework
import * as moment from "moment";

export const getUsDate = (date: Date) => formatTime(date, "L")

export const getUsTime = (date: Date) => formatTime(date, "hh:mm:ss A")

const formatTime = (date: Date, format: string) => {
	const momentizedDate = moment.utc(date).local();
	return momentizedDate.format(format)
}

export const getFormattedTimeDistance = (date: Date) => {
	const momentizedDate = moment.utc(date).local();
	const diff = moment.duration(momentizedDate.diff(moment().local()));

	let diffUnit = Math.abs(diff.asMinutes());
	if (diffUnit < 60) {
		return `${diffUnit.toFixed(0)}min`;
	}

	diffUnit = Math.abs(diff.asHours());
	if (diffUnit < 24) {
		return `${diffUnit.toFixed(0)}h`;
	}

	diffUnit = Math.abs(diff.asDays());
	if (diffUnit < 7) {
		return `${diffUnit.toFixed(0)}d`;
	}

	diffUnit = Math.abs(diff.asWeeks());
	if (diffUnit < 4) {
		return `${diffUnit.toFixed(0)}w`;
	}

	diffUnit = Math.abs(diff.asMonths());
	if (diffUnit < 12) {
		return `${diffUnit.toFixed(0)}m`;
	}

	diffUnit = Math.abs(diff.asYears());
	return `${diffUnit.toFixed(0)}y`;
}