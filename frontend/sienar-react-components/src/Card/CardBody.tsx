import { useContext } from 'react';
import { CardContext } from './CardContext';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { HTMLAttributes, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';

type CardBodyProps = Themeable & PropsWithChildren & HTMLAttributes<HTMLDivElement>;

export default function CardBody(props: CardBodyProps) {
	const cardContext = useContext(CardContext);

	const {
		color = cardContext.color,
		variant = cardContext.variant
	} = props;

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLDivElement>;
	cleanedProps.className = createThemedClassNames(color, variant, 'card__body');

	return <div {...cleanedProps}/>;
}