import { useState, useEffect, useContext, useRef } from 'react';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';
import TableContext from './TableContext';

import type { HTMLAttributes } from 'react';
import type { TableProps, TableState } from './types';

import { renderTableContent } from './renderers';

export default function Table<T>(props: TableProps<T>) {
	const { items,	onTableStateChanged} = props;

	const [itemsToDisplay, setItemsToDisplay] = useState<T[]>([]);
	// const tableContext = useRef(useContext(TableContext));
	const tableContext = useContext(TableContext);
	useEffect(
		() => {
			(async () => {
				if (items) {
					// TODO: perform filtering
					setItemsToDisplay(items);
					return;
				}

				if (onTableStateChanged) {
					const tableState: TableState = {
						page: tableContext.page,
						pageSize: tableContext.pageSize,
						sortColumn: tableContext.sortColumn,
						sortDescending: tableContext.sortDescending
					};
					const result = await onTableStateChanged(tableState);
					setItemsToDisplay(result.items ?? []);
					tableContext.setCount(result.totalCount ?? 0);
				}
			})()
		},
		[tableContext, items]
	);

	const cleanedProps = cleanThemeFromProps(props) as HTMLAttributes<HTMLTableElement>;
	cleanedProps.className = createThemedClassNames(tableContext.color, tableContext.variant, 'table');

	if (!cleanedProps.children) {
		cleanedProps.children = renderTableContent<T>(props);
	}

	return <table {...cleanedProps}/>;
}