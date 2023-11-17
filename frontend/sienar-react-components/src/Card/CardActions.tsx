import { useContext } from 'react';
import { CardContext } from './CardContext';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { HTMLAttributes, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';

type ActionsProps = Themeable & PropsWithChildren & HTMLAttributes<HTMLDivElement>;

export default function CardActions(props: ActionsProps) {
	const cardContext = useContext(CardContext);

	const {
		color = cardContext.color,
		variant = cardContext.variant
	} = props;

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLDivElement>;
	cleanedProps.className = createThemedClassNames(color, variant,
		'card__actions');

	return <div {...cleanedProps} />;
}