import Table from '@/components/ui/Table';
import {stateService} from '../services';
import {useNavigate} from 'react-router';
import links from '../links';
import { Icon } from '@sienar/react-components';
import type { ColumnDef } from '@sienar/react-components/src/Table/types';
import type { StateDto } from '../services';

const columns: ColumnDef<StateDto>[] = [
	{
		field: 'name',
		headerName: 'State name'
	},
	{
		field: 'abbreviation',
		headerName: 'State abbreviation'
	}
];

export default function Index() {
	const navigate = useNavigate();

	return (
		<Table
			title='States'
			actionButton='plus'
			onActionButtonClick={() => navigate(links.add)}
			columns={columns}
			fetchResults={stateService.getAll}
			onEditButtonClick={(state: StateDto) => navigate(`${links.edit}/${state.id}`)}
			onDeleteButtonClick={(state: StateDto) => stateService.delete(state.id!)}
		/>
	);
}