import AuthorizeRoute from '@/components/auth/AuthorizeRoute';
import roles from '@/roles';

import Index from './views/Index';
import Upsert from './views/Upsert';
import {createCrudRouter} from '@/utils/routerUtils';

const indexElement = (
	<AuthorizeRoute roles={roles.admin}>
		<Index/>
	</AuthorizeRoute>
);

const upsertElement = (
	<AuthorizeRoute roles={roles.admin}>
		<Upsert/>
	</AuthorizeRoute>
);

export default createCrudRouter(
	'states',
	indexElement,
	upsertElement,
	upsertElement
);