// Framework
import * as moment from "moment";

export const getUsDate = (date: Date) => {
	const momentizedDate = moment(date);
	return momentizedDate.format("L");
}

export const getUsTime = (date: Date) => {
	const momentizedDate = moment(date);
	return momentizedDate.format("hh:mm:ss")
}