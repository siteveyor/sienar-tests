import Table from '@/components/ui/Table';
import { service, logout, SienarUserDto } from '@account/services';
import {useNavigate} from 'react-router';
import links from '@account/links';
import { useSelector } from 'react-redux';
import { RootState } from '@/store';
import { ColumnDef } from '@sienar/react-components/src/Table/types';

const columns: ColumnDef<SienarUserDto>[] = [
	{
		field: 'username',
		headerName: 'Username'
	},
	{
		field: 'email',
		headerName: 'Email address'
	}
];

export default function Index() {
	const navigate = useNavigate();
	const currentUserId = useSelector((state: RootState) => state.appData.user.id!);

	const onDelete = async (user: SienarUserDto) => {
		const result = await service.delete(user.id!);
		if (result && user.id === currentUserId) {
			await logout();
		}
	}

	return (
		<Table
			title='Users'
			actionButton='plus'
			onActionButtonClick={() => navigate(links.admin.add)}
			columns={columns}
			fetchResults={service.getAll}
			onEditButtonClick={(user: SienarUserDto) => navigate(`${links.admin.edit}/${user.id}`)}
			onDeleteButtonClick={onDelete}
			// hideSearch
		/>
	);
}