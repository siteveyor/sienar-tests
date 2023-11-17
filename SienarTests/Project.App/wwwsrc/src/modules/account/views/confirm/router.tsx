import type {RouteObject} from 'react-router';

import Index from './Index';
import Successful from './Successful';
import AuthorizeRoute from '@/components/auth/AuthorizeRoute';

const router: RouteObject = {
	path: 'confirm',
	children: [
		{
			path: '',
			element: (
				<AuthorizeRoute mustBeLoggedOut>
					<Index/>
				</AuthorizeRoute>
			)
		},
		{
			path: 'successful',
			element: (
				<AuthorizeRoute mustBeLoggedOut>
					<Successful/>
				</AuthorizeRoute>
			)
		}
	]
};

export default router;