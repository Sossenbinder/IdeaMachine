const formatter = new Intl.NumberFormat();

export const formatAmount = (amount: string) => {
	return formatter.format(+amount);
}