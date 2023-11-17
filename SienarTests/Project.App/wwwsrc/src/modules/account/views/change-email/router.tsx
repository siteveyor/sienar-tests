import type {RouteObject} from 'react-router';

import Index from './Index';
import Confirm from './Confirm';
import Requested from './Requested';
import Successful from './Successful';
import AuthorizeRoute from '@/components/auth/AuthorizeRoute';

const router: RouteObject = {
	path: 'change-email',
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
			path: 'confirm',
			element: (
				<AuthorizeRoute>
					<Confirm/>
				</AuthorizeRoute>
			)
		},
		{
			path: 'requested',
			element: (
				<AuthorizeRoute>
					<Requested/>
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