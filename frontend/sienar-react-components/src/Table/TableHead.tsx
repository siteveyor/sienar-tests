import { useContext } from 'react';
import TableContext from './TableContext';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';

import type { Themeable } from '@/types';
import type { HTMLAttributes, PropsWithChildren } from 'react';

type TableHeadProps = Themeable & PropsWithChildren & HTMLAttributes<HTMLTableSectionElement>;

export default function TableHead(props: TableHeadProps) {
	const tableContext = useContext(TableContext);

	const {
		color = tableContext.color,
		variant = tableContext.variant
	} = props;

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLElement>;
	cleanedProps.className = createThemedClassNames(color, variant, 'table__head');

	return <thead {...cleanedProps}/>;
}