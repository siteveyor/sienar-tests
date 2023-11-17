import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { HTMLAttributes, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';

import ListItem from './ListItem';
import ListItemButton from './ListItemButton';
import ListItemIcon from './ListItemIcon';
import ListItemLink from './ListItemLink';
import ListItemText from './ListItemText';

import { ListContext } from './ListContext'

type ListProps = {
	type?: 'default' | 'nav' | 'blank'
}
	& Themeable
	& PropsWithChildren
	& HTMLAttributes<HTMLUListElement>

function List(props: ListProps) {
	const {
		type = 'default',
		color = 'default',
		variant = 'solid'
	} = props;

	const cleanedProps = cleanThemeFromProps(props, 'type') as HTMLAttributes<HTMLUListElement>;
	cleanedProps.className = `${createThemedClassNames(color, variant, 'list')} list--${type}`;

	return (
		<ListContext.Provider value={{ color, variant }}>
			<ul {...cleanedProps}/>
		</ListContext.Provider>
	);
}

export default Object.assign(List, {
	Item: ListItem,
	ItemButton: ListItemButton,
	ItemIcon: ListItemIcon,
	ItemLink: ListItemLink,
	ItemText: ListItemText
});