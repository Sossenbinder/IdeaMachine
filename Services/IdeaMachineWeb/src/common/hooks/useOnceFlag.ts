import { useState } from "react";

export const useOnceFlag = (): [boolean, () => void, () => void] => {
	const [flag, setFlag] = useState(false);

	const set = () => setFlag(true);

	const reset = () => setFlag(false);

	return [flag, set, reset];
}

export default useOnceFlag;