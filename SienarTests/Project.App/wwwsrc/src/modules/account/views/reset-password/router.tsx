import Index from './Index';
import Successful from './Successful';
import AuthorizeRoute from '@/components/auth/AuthorizeRoute';
import {RouteObject} from 'react-router';

const router: RouteObject = {
	path: 'reset-password',
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