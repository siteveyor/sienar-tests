import { Table as SienarTable, Button, Form } from '@sienar/react-components';
import {MouseEventHandler, ReactNode, useEffect, useState} from 'react';
import {FilterDto, PagedDto} from '@/utils/types';
import type { ColumnDef, PagedResult, TableState } from '@sienar/react-components/src/Table/types';

interface Props<T> {
	title: string
	hideSearch?: boolean
	columns: ColumnDef<T>[]
	hideActions?: boolean
	hideEdit?: boolean
	onEditButtonClick?: (item: T) => any
	hideDelete?: boolean
	onDeleteButtonClick?: (item: T) => any
	actionRenderer?: (item: T) => ReactNode
	actionButton?: string
	onActionButtonClick?: MouseEventHandler<HTMLButtonElement>
	fetchResults: (filter: FilterDto) => Promise<PagedDto<T>>
}

export default function Table<T>(props: Props<T>) {
	const {
		title,
		hideSearch = false,
		columns,
		hideActions = false,
		hideEdit = false,
		onEditButtonClick,
		hideDelete = false,
		onDeleteButtonClick,
		actionRenderer,
		actionButton,
		onActionButtonClick,
		fetchResults
	} = props;

	const [searchTerm, setSearchTerm] = useState('');
	const [isLoading, setIsLoading] = useState(false);
	const [triggerRender, setTriggerRender] = useState(false);

	// Set up data loading
	const loadResults = async (state: TableState): Promise<PagedResult<T>> => {
		const filter: FilterDto = {
			searchTerm,
			pageSize: state.pageSize,
			sortColumnName: state.sortColumn,
			page: state.page,
			sortDescending: state.sortDescending
		};

		setIsLoading(true);
		const result = await fetchResults(filter);
		setIsLoading(false);
		return result;
	};

	const handleEditClicked = async (item: T) => {
		await onEditButtonClick?.(item);
	}

	const handleDeleteClicked = async (item: T) => {
		await onDeleteButtonClick?.(item);

		// If we just call loadResults() here, what happens is that it calls loadResults()
		// AS IT EXISTED WHEN THIS CONSTANT WAS SET UP. So when an item is actually deleted,
		// what happens is, the first page of results is loaded with all default options,
		// but the table state isn't reset. We want to load data with the current table settings
		// so in order to do that, we need to arbitrarily trigger our useEffect
		setTriggerRender(!triggerRender);
	}

	// Set up columns
	if (!columns.find(c => c.field === 'id')) {
		columns.unshift({
			field: 'id',
			headerName: 'ID',
			sortable: false
		});
	}

	if (!hideActions) {
		// Remove existing actions - we need this to be fresh
		const i = columns.findIndex(c => c.field === 'actions')
		if (i > -1) {
			columns.splice(i, 1);
		}

		columns.push({
			field: 'actions',
			headerName: 'Actions',
			sortable: false,
			renderCell: (value) => (
				<>
					{actionRenderer?.(value)}
					{!hideEdit && (
						<Button.Icon
							icon='pencil'
							color='warning'
							onClick={() => handleEditClicked(value)}
						/>
					)}
					{!hideDelete && (
						<Button.Icon
							icon='trash'
							color='error'
							onClick={() => handleDeleteClicked(value)}
						/>
					)}
				</>
			)
		});
	}

	// columns.forEach(c => {
	// 	if (c.field === 'id') {
	// 		c.width = 50;
	// 	} else {
	// 		c.minWidth = 150;
	// 		c.flex = 1;
	// 	}
	// });

	const resetButton = searchTerm && (
		<Button.Icon
			icon='xmark'
			size='2xs'
			color='white'
			onClick={() => setSearchTerm('')}
		/>
	);

	// // All filter variables should trigger an immediate table reload...
	// useEffect(() => {loadResults()}, [pageSize, sortColumnName, page, sortDescending, triggerRender]);

	// ...except searchTerm, which is typed and should be debounced
	useEffect(() => {
		const id = setTimeout(loadResults, 500);
		return () => clearTimeout(id);
	}, [searchTerm]);

	// const handleSortModelChange = (sortModel: GridSortModel) => {
	// 	if (sortModel.length < 1) {
	// 		setSortColumnName('');
	// 		setSortDescending(false);
	// 		return;
	// 	}
	//
	// 	setSortColumnName(sortModel[0].field);
	// 	setSortDescending(sortModel[0].sort === 'desc');
	// };

	// @ts-ignore
	return (
		<div>
			{/* Header box */}
			<div
				// sx={{
				// 	bgcolor: 'primary.main',
				// 	color: 'white',
				// 	px: 3,
				// 	py: 2,
				// 	display: 'flex',
				// 	justifyContent: 'space-between',
				// 	alignItems: 'center'
				// }}
			>
				<h6>{title}</h6>
				<div
					// sx={{display: 'flex'}}
				>
					{!hideSearch && (
						<Form.TextInput
							// color='white'
							// startAdornment={<Icon icon='magnifying-glass'/>}
							// endAdornment={resetButton}
							value={searchTerm}
							setValue={setSearchTerm}
							// onChange={e => setSearchTerm(e.target.value)}
						/>
					)}

					{actionButton && (
						<Button.Icon
							icon={actionButton}
							color='white'
							onClick={onActionButtonClick}
							// sx={{p: 0, ml: 4}}
						/>
					)}
				</div>
			</div>

			{/* Main content */}
			<SienarTable
				columns={columns}
				// @ts-ignore
				onTableStateChanged={loadResults}
			/>
			{/*<DataGrid*/}
			{/*	rows={rows}*/}
			{/*	rowCount={rowCount}*/}
			{/*	rowsPerPageOptions={[5, 10, 25]}*/}
			{/*	page={page - 1}*/}
			{/*	onPageChange={newPage => setPage(newPage + 1)}*/}
			{/*	pageSize={pageSize}*/}
			{/*	onPageSizeChange={newPageSize => setPageSize(newPageSize)}*/}
			{/*	onSortModelChange={handleSortModelChange}*/}
			{/*	loading={isLoading}*/}
			{/*	paginationMode='server'*/}
			{/*	sortingMode='server'*/}
			{/*	autoHeight*/}
			{/*/>*/}
		</div>
	);
}