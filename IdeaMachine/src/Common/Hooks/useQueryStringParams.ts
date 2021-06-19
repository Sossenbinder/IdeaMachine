// Framework
import * as React from "react";
import { useLocation } from "react-router-dom";

export const useQueryStringParams = (): URLSearchParams => {
	const [urlParams] = React.useState<URLSearchParams>(new URLSearchParams(useLocation().search));
	return urlParams;
}

export default useQueryStringParams;