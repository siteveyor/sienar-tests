import type {RouteObject} from 'react-router';
import AuthorizeRoute from '@/components/auth/AuthorizeRoute';

import Login from './views/Login';
import PersonalData from './views/PersonalData';
import Deleted from './views/Deleted';

import adminRoutes from './views/admin-panel/router';
import changeEmailRoutes from './views/change-email/router';
import changePasswordRoutes from './views/change-password/router';
import confirmRoutes from './views/confirm/router';
import forgotPasswordRoutes from './views/forgot-password/router';
import registerRoutes from './views/register/router';
import resetPasswordRoutes from './views/reset-password/router';

const router: RouteObject = {
	children: [
		{
			path: 'account',
			children: [
				{
					path: 'login',
					element: (
						<AuthorizeRoute mustBeLoggedOut>
							<Login/>
						</AuthorizeRoute>
					)
				},
				{
					path: 'personal-data',
					element: (
						<AuthorizeRoute>
							<PersonalData/>
						</AuthorizeRoute>
					)
				},
				{
					path: 'deleted',
					element: <Deleted/>
				},
				changeEmailRoutes,
				changePasswordRoutes,
				confirmRoutes,
				forgotPasswordRoutes,
				registerRoutes,
				resetPasswordRoutes
			]
		},
		adminRoutes
	]
};

export default router;