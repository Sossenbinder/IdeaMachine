// Framework
import * as React from "react";

// Components
import NavBar from 'common/Components/Page/NavBar';
import Flex from "common/Components/Flex";
import Content from "./Content";

// Functionality
import { getTranslations } from 'common/Translations/translations';

// Styles
import styles from "./Styles/MainContainer.module.less";

type Props = {

}

export const MainContainer: React.FC<Props> = () => {

	const [loading, setLoading] = React.useState<boolean>(true);

	const data = getTranslations();

	React.useEffect(() => {
		if (data) {
			setLoading(false);
		}
	}, [data]);

	return (
		<Flex
			className={styles.MainContainer}
			direction="Column">
			<If condition={loading}>
				Loading...
			</If>
			<If condition={!loading}>
				<Flex
					className={styles.MainGrid}
					direction="Column">
					<NavBar />
					<Content />
				</Flex>
			</If>
		</Flex>
	);
}

export default MainContainer;