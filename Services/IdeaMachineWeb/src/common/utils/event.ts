import { SyntheticEvent } from "react";

export const withSuppressedEvent = <TEvent extends SyntheticEvent<any, any>>(event: TEvent, cb: (event: TEvent) => void | Promise<void>) => {
	event.stopPropagation();
	event.preventDefault();
	cb(event);
};
