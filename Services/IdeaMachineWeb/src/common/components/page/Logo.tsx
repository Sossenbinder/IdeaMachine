import * as React from "react";

type Props = {
	size?: "s" | "l";
};

export default function Logo({ size = "l" }: Props) {
	return <img style={{ height: `${size === "l" ? 40 : 20}px` }} src="/Resources/Icons/ideablub.svg" />;
}
