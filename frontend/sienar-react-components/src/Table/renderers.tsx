import Head from './TableHead';
import HeaderCell from './TableHeaderCell';
import type { TableProps } from './types';
import type { ReactNode } from 'react';

export function renderTableContent<T>(props: TableProps<T>) {
	const headerContent = renderHeader<T>(props);

	return (
		<>
			{headerContent}
		</>
	);
}

export function renderHeader<T>(props: TableProps<T>) {
	// if (!props.items || props.items.length === 0) {
	// 	return;
	// }

	let headerContent: ReactNode = '';
	if (props.headRowRenderer) {
		headerContent = props.headRowRenderer();
	} else if (props.columns && props.columns.length > 0) {
		headerContent = (
			<>
				{props.columns.map(c => {
					const headerCellContent = c.renderHeaderCell
						? c.renderHeaderCell()
						: `${c.headerName ?? c.field}`
					return (
						<HeaderCell>
							{headerCellContent}
						</HeaderCell>
					);
				})}
			</>
		);
	}

	return (
		<Head>
			{headerContent}
		</Head>
	)
}

export function renderRow<T>(props: TableProps<T>) {
	
}