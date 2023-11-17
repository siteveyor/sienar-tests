import { useContext } from 'react';
import ListItem from './ListItem';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';
import { ListContext } from '@/List/ListContext';
import Button from '@/Button';

import type { JSX, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';
import type { ButtonProps } from '@/Button/Button';

export type ListItemButtonProps = {
	nestedList?: JSX.Element
} & Themeable & PropsWithChildren & ButtonProps;

export default function ListItemButton(props: ListItemButtonProps) {
	const listContext = useContext(ListContext);

	const {
		color = listContext.color,
		variant = listContext.variant,
		nestedList
	} = props;

	const cleanedProps = cleanThemeFromProps(props, 'nestedList') as ButtonProps;
	cleanedProps.className = createThemedClassNames(color, variant, 'list__list-item-button');

	return (
		<ListItem 
			className='list__list-item--button'
			color={color}
			variant={variant}
			nestedList={nestedList}
			suppressWrapper
		>
			<Button
				{...cleanedProps}
				suppressTheme
			/>
		</ListItem>
	);
}