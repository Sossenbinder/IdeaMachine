// Framework
import * as React from "react";

// Types
import { OrderType } from "modules/Ideas/types";

type IdeaFilterState = {
	order: OrderType;
	updateOrder: (orderType: OrderType) => void;
}

const defaultValue: IdeaFilterState = {
	order: OrderType.Created,
	updateOrder: _ => undefined,
}

export const IdeaFilterContext = React.createContext(defaultValue);

export const IdeaFilterContextProvider: React.FC = ({ children }) => {

	const [filter, setFilter] = React.useState(OrderType.Created);

	return (
		<IdeaFilterContext.Provider value={{
			order: filter,
			updateOrder: setFilter,
		}}>
			{children}
		</IdeaFilterContext.Provider>
	)
}

export default IdeaFilterContextProvider;