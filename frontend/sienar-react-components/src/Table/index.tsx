import { useState } from 'react';
import { cleanThemeFromProps, createThemedClassNames } from '@/utils';
import TableContext from './TableContext';

import type { TableProps } from './types';

import Table from './Table';
import Head from './TableHead';
import HeaderCell from './TableHeaderCell';
import Body from './TableBody';
import Cell from './TableCell';

function TableWrapper<T>(props: TableProps<T>) {
	// const {
	// 	color = 'default',
	// 	variant = 'outlined',
	// 	page = 1,
	// 	pageSize = 10,
	// 	items,
	// 	onTableStateChanged
	// } = props;
	// const count = props.count ?? items?.length ?? 0;
	const {
		color = 'default',
		variant = 'outlined'
	} = props;

	const [page, setPage] = useState(props.page ?? 1);
	const [pageSize, setPageSize] = useState(props.pageSize ?? 10);
	const [count, setCount] = useState(props.items?.length ?? 0);
	const [sortColumn, setSortColumn] = useState<string|null>(null);
	const [sortDescending, setSortDescending] = useState(false);

	const cleanedProps = cleanThemeFromProps(props, 'page', 'pageSize', 'count') as TableProps<T>;

	return (
		<TableContext.Provider value={{
			color,
			variant,
			page,
			pageSize,
			count,
			sortColumn,
			sortDescending,
			setPage,
			setPageSize,
			setCount,
			setSortColumn,
			setSortDescending
		}}>
			<Table {...cleanedProps}/>
		</TableContext.Provider>
	)
}

export default Object.assign(TableWrapper, {
	Head,
	HeaderCell,
	Body,
	Cell
});