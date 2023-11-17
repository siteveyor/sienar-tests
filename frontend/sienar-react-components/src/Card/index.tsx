import { cleanThemeFromProps, createThemedClassNames } from '@/utils';
import {CardContext} from './CardContext';

import Header from './CardHeader';
import Body from './CardBody';
import Actions from './CardActions';
import type { HTMLAttributes, PropsWithChildren } from 'react';
import type {Themeable} from '@/types';

type CardProps = {
	tag?: 'article'|'section'|'div'
} & Themeable & PropsWithChildren & HTMLAttributes<HTMLElement>

function Card(props: CardProps) {
	const {
		color = 'default',
		variant = 'outlined'
	} = props;

	const Tag = props.tag ?? 'div';
	const cleanedProps = cleanThemeFromProps(props, 'tag') as HTMLAttributes<HTMLDivElement>;
	cleanedProps.className = createThemedClassNames(color, variant, 'card');

	return (
		<CardContext.Provider value={{ color, variant }}>
			<Tag {...cleanedProps}/>
		</CardContext.Provider>
	);
}

export default Object.assign(Card, {
	Header,
	Body,
	Actions
});