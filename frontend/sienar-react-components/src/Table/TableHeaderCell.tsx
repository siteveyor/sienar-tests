import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { Themeable } from '@/types';
import type { HTMLAttributes, PropsWithChildren } from 'react';

type TableHeaderCellProps = Themeable & PropsWithChildren & HTMLAttributes<HTMLTableCellElement>;

export default function TableHeaderCell(props: TableHeaderCellProps) {
	const {
		color = 'default',
		variant = 'text'
	} = props;

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLTableCellElement>;
	cleanedProps.className = createThemedClassNames(color, variant, 'table__header-cell');

	return <th {...cleanedProps}/>;
}