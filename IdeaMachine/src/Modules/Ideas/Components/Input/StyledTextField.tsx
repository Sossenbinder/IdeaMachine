// Framework
import { styled } from "@material-ui/core/styles";
import { TextField } from "@material-ui/core";

export const StyledTextField = styled(TextField)(() => ({
	["& .MuiInputBase-input"]: {
		color: "white",
	},
	["& .MuiInputBase-root"]: {
		flexDirection: "column"
	},
	["& .MuiFormLabel-root"]: {
		color: "white",
	},
	["& .MuiInput-underline:before"]: {
		borderBottomColor: "black",
	},
}));

export default StyledTextField;