import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { HTMLAttributes, PropsWithChildren } from 'react';
import type { Themeable } from '@/types';

type ListItemTextProps = Pick<Themeable, 'color'> & PropsWithChildren & HTMLAttributes<HTMLDivElement>

export default function ListItemText(props: ListItemTextProps) {
	// This component purposely doesn't use the ListContext
	// Instead, the color can be set independently of the list
	// because some logical inconsistencies can occur if we set them together
	// For example, if the list is both color: primary and variant: solid,
	// then we probably want the text to be either white or black
	// But the class name in that case would be list__list-item-text--primary
	// which would seem to imply we want the text to be the primary color
	// To avoid this confusion, this component ignores the context completely
	// Devs should set this color with descendent selectors based on the list theme color
	const { color } = props;

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLDivElement>;
	cleanedProps.className = createThemedClassNames(color, null, 'list__list-item-text');

	return <div {...cleanedProps}/>;
}