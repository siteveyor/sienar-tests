import { useContext } from 'react';
import { CardContext } from './CardContext';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { HTMLAttributes, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';

type CardHeaderProps = Themeable & PropsWithChildren & HTMLAttributes<HTMLElement>;

// bgcolor: 'primary.main',
// color: 'white.main',
// px: 3,
// py: 2,
// display: 'flex',
// justifyContent: 'space-between',
// alignItems: 'center'
export default function CardHeader(props: CardHeaderProps) {
	const cardContext = useContext(CardContext);

	const {
		color = cardContext.color,
		variant = cardContext.variant
	} = props;

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLElement>;
	cleanedProps.className = createThemedClassNames(color, variant, 'card__header');

	return <header {...cleanedProps}/>;
}