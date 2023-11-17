import { createContext } from 'react';
import type { Themeable } from '@/types';

export type TableContextType = {
	page: number
	pageSize: number
	count: number
	sortColumn: string|null
	sortDescending: boolean
	setPage: (newPage: number) => void
	setPageSize: (newPageSize: number) => void
	setCount: (newCount: number) => void
	setSortColumn: (newSortColumn: string|null) => void
	setSortDescending: (newSortDescending: boolean) => void
} & Themeable;

export default createContext<TableContextType>({
	page: 1,
	pageSize: 10,
	count: 0,
	sortColumn: '',
	sortDescending: false,
	setPage: p => {},
	setPageSize: s => {},
	setCount: c => {},
	setSortColumn: c => {},
	setSortDescending: d => {}
});