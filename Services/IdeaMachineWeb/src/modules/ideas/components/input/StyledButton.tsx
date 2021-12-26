// Framework
import Button from "@mui/material/Button";
import { styled } from "@mui/system";

export const StyledButton = styled(Button)(() => ({
	"& .Mui-disabled": {
		backgroundColor: "white",
		color: "red"
	},
	".MuiButton-root": {
		border: "1px solid black"
	}
}));

export default StyledButton;
