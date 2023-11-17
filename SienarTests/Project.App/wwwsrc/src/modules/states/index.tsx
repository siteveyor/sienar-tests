import {Module} from '@/utils/types';
import router from './router';
import links from './links';
import roles from '@/roles';
import {Priority} from '@/utils/general';
import { Icon } from '@sienar/react-components';

const module: Module = {
	router,
	drawerItems: [
		{
			text: 'States',
			to: links.index,
			authorization: roles.admin,
			priority: Priority.low,
			icon: <Icon icon='landmark'/>
		}
	]
};

export default module;