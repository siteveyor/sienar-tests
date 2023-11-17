import type { Themeable } from '@/types';
import type { HTMLAttributes, PropsWithChildren } from 'react';
import { classNames, cleanThemeFromProps, createThemedClassNames } from '@/utils';

type AppBarProps = {
	tag?: 'header'|'article'|'section'|'div',
	fixed?: boolean
} & Pick<Themeable, 'color'> & PropsWithChildren & HTMLAttributes<HTMLElement>

function AppBar(props: AppBarProps) {
	const {
		color = 'light',
		fixed = false
	} = props;

	const Tag = props.tag ?? 'div';
	const cleanedProps = cleanThemeFromProps(props, 'fixed', 'tag') as HTMLAttributes<HTMLElement>;
	cleanedProps.className = classNames(
		createThemedClassNames(color, null, 'app-bar'),
		{
			'app-bar--fixed': fixed
		}
	);

	return <Tag {...cleanedProps}/>;
}

export default Object.assign(AppBar, {});