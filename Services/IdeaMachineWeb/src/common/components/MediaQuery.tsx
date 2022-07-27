// Framework
import React from "react";
import { useMediaQuery } from "react-responsive";

export const Desktop = ({ children }: { children: React.ReactChildren }) => {
	const isDesktop = useMediaQuery({ minWidth: 992 });
	return isDesktop ? children : null;
};

export const Tablet = ({ children }: { children: React.ReactChildren }) => {
	const isTablet = useMediaQuery({ minWidth: 768, maxWidth: 991 });
	return isTablet ? children : null;
};

export const Mobile = ({ children }: { children: React.ReactChildren }) => {
	const isMobile = useMediaQuery({ maxWidth: 767 });
	return isMobile ? children : null;
};

export const Default = ({ children }: { children: React.ReactChildren }) => {
	const isNotMobile = useMediaQuery({ minWidth: 768 });
	return isNotMobile ? children : null;
};

export const TabletAndBelow = ({ children }: { children: React.ReactChildren }) => {
	const isTabletAndBelow = useMediaQuery({ maxWidth: 1279 });
	return isTabletAndBelow ? children : null;
};
