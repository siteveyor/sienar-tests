import { NavLink } from 'react-router-dom';
import { useContext } from 'react';
import ListItem from './ListItem';
import { ListContext } from '@/List/ListContext';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { NavLinkProps } from 'react-router-dom';
import type { Themeable } from '@/types';

type ListItemLinkProps = Themeable & NavLinkProps

export default function ListItemLink(props: ListItemLinkProps) {
	const listContext = useContext(ListContext);

	const {
		color = listContext.color,
		variant = listContext.variant
	} = props;

	const cleanedProps = cleanThemeFromProps(props) as NavLinkProps;
	cleanedProps.className = createThemedClassNames(color, variant, 'list__list-item-link')

	return (
		<ListItem 
			className='list__list-item--link'
			color={color}
			variant={variant}
		>
			<NavLink {...cleanedProps}/>
		</ListItem>
	)
}