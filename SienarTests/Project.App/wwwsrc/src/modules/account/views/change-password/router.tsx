import type {RouteObject} from 'react-router';

import Index from './Index';
import Successful from './Successful';
import AuthorizeRoute from '@/components/auth/AuthorizeRoute';

const router: RouteObject = {
	path: 'change-password',
	children: [
		{
			path: '',
			element: (
				<AuthorizeRoute>
					<Index/>
				</AuthorizeRoute>
			)
		},
		{
			path: 'successful',
			element: (
				<AuthorizeRoute>
					<Successful/>
				</AuthorizeRoute>
			)
		}
	]
};

export default router;