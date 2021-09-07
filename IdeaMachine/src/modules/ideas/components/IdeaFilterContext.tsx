// Framework
import * as React from "react";

// Types
import { OrderType, OrderDirection } from "modules/ideas/types";

type IdeaFilters = {
	order: OrderType;
	direction: OrderDirection;
	tags: Array<string>;
}

type IdeaFilterState = {
	filters: IdeaFilters;
	updateFilters: (ideaFilters: IdeaFilters) => void;
}

const defaultValue: IdeaFilterState = {
	filters: {
		order: OrderType.Created,
		direction: OrderDirection.Down,
		tags: new Array<string>(),
	},
	updateFilters: _ => undefined,
}

export const IdeaFilterContext = React.createContext(defaultValue);

export const IdeaFilterContextProvider: React.FC = ({ children }) => {

	const [filters, updateFilters] = React.useState({
		order: OrderType.Created,
		direction: OrderDirection.Down,
		tags: new Array<string>(),
	});

	return (
		<IdeaFilterContext.Provider value={{
			filters,
			updateFilters,
		}}>
			{children}
		</IdeaFilterContext.Provider>
	)
}

export default IdeaFilterContextProvider;