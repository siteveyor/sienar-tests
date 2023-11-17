import { useContext } from 'react';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';
import { ListContext } from '@/List/ListContext';

import type { HTMLAttributes, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';

type ListItemIconProps = {
	position?: 'start' | 'end'
}
	& Themeable
	& PropsWithChildren
	& HTMLAttributes<HTMLDivElement>;

export default function ListItemIcon(props: ListItemIconProps) {
	const listContext = useContext(ListContext);

	const {
		color = listContext.color,
		variant = listContext.variant,
		position = 'start'
	} = props;

	const cleanedProps = cleanThemeFromProps(props, 'position') as HTMLAttributes<HTMLDivElement>;
	cleanedProps.className = `${createThemedClassNames(color, variant, 'list__list-item-icon')} list__list-item-icon--${position}`;

	return <div {...cleanedProps}/>;
}