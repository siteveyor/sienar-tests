import { useContext } from 'react';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';
import { ListContext } from './ListContext';

import type { HTMLAttributes, PropsWithChildren, JSX } from 'react';
import type { Themeable } from '@/types';

type ListItemProps = {
	nestedList?: JSX.Element
	suppressWrapper?: boolean
} & Themeable & PropsWithChildren & HTMLAttributes<HTMLElement>;

export default function ListItem(props: ListItemProps) {
	const listContext = useContext(ListContext);
	const {
		color = listContext.color,
		variant = listContext.variant,
		suppressWrapper = false,
		nestedList,
		children
	} = props;

	const cleanedProps = cleanThemeFromProps(props, 'suppressWrapper', 'children', 'nestedList') as HTMLAttributes<HTMLElement>;
	cleanedProps.className = createThemedClassNames(color, variant, 'list__list-item');

	return (
		<li className='list__list-item-wrapper'>
			{
				(!suppressWrapper && <div {...cleanedProps}>{children}</div>)
				|| children
			}
			{nestedList && <div className='list__nested-list'>{nestedList}</div>}
		</li>
	);
}