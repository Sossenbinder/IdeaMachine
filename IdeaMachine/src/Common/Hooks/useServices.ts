// Framework
import * as React from "react";

// Functionality
import ServiceContext from "common/Modules/Service/ServiceContext";

// Types
import { Services } from "common/modules/Service/types";

export const useServices = (): Services => {
	return React.useContext(ServiceContext);
}

export default useServices;