// Framework
import { TextField } from "@material-ui/core";
import { styled } from "@material-ui/core/styles";

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
	["& .MuiOutlinedInput-input"]: {
		marginLeft: "15px",
	},
	["& .MuiInputBase-multiline"]: {
		height: "100%",
	}
}));

export default StyledTextField;