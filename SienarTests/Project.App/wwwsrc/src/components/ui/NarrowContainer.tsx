import {Container} from '@sienar/react-components';
import type {PropsWithChildren} from 'react';

export default function NarrowContainer({children}: PropsWithChildren) {
	return (
		<Container maxWidth='sm'>
			{children}
		</Container>
	);
}