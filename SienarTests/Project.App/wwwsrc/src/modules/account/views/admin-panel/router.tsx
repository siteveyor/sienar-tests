import AuthorizeRoute from '@/components/auth/AuthorizeRoute';
import roles from '@/roles';
import { RouteObject } from 'react-router';
import { createCrudRouter } from '@/utils/routerUtils';

import Index from './Index';
import Upsert from './Upsert';

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

const router: RouteObject = createCrudRouter(
	'users',
	indexElement,
	upsertElement,
	upsertElement
);

export default router;