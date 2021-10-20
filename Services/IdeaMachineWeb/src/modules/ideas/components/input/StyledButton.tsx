// Framework
import { Button } from "@material-ui/core";
import { styled } from "@material-ui/core/styles";

export const StyledButton = styled(Button)(() => ({
	"& .Mui-disabled": {
		backgroundColor: "white",
		color: "red",
	},
	".MuiButton-root": {
		border: "1px solid black",
	},
}));

export default StyledButton;