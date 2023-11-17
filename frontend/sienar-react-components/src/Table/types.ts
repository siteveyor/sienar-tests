import type { JSX } from 'react';
import type { Blank, Themeable } from '@/types';
import type { HTMLAttributes, PropsWithChildren } from 'react';

export type TableProps<T> = {
	page?: number
	pageSize?: number
	items?: T[]
	columns?: ColumnDef<T>[]
	headRowRenderer?: () => JSX.Element
	rowRenderer?: (item: T) => JSX.Element|null
	onTableStateChanged?: (state: TableState) => Promise<PagedResult<T>>
} & Themeable & PropsWithChildren & HTMLAttributes<HTMLTableElement>;

export type ColumnDef<T> = {
	colspan?: number
	field: string
	headerName?: string
	renderCell?: (item: T) => JSX.Element | null
	renderHeaderCell?: () => JSX.Element | null
	sortable?: boolean
};

export type PagedResult<T> = {
	items?: T[]|null
	totalCount?: number|null
};

export type TableState = {
	page: number
	pageSize: number
	sortColumn: string|Blank
	sortDescending: boolean
};