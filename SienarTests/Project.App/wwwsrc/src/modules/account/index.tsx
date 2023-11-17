import {Module} from '@/utils/types';
import router from './router';
import links from '@account/links';
import {Priority} from '@/utils/general';
import roles from '@/roles';
import { Icon } from '@sienar/react-components';

const module: Module = {
	router,
	drawerItems: [
		{
			text: 'Users',
			to: links.admin.index,
			authorization: roles.admin,
			priority: Priority.low,
			icon: <Icon icon='user-group'/>
		},
		{
			text: 'My account',
			icon: <Icon icon='gear'/>,
			authorization: true,
			priority: Priority.lowest,
			children: [
				{
					text: 'Email',
					to: links.changeEmail.index,
					icon: <Icon icon='envelope'/>
				},
				{
					text: 'Password',
					to: links.changePassword.index,
					icon: <Icon icon='key'/>
				},
				{
					text: 'Personal data',
					to: links.personalData.index,
					icon: <Icon icon='shield-halved'/>
				}
			]
		}
	]
};

export default module;