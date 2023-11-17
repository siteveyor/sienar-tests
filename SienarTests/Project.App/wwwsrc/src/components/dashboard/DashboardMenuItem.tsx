import { cleanProps, List } from '@sienar/react-components';
import {DrawerItem} from '@/utils/types';
import {useDrawerItemAuthorized} from '@/utils/hooks';

import type { ListItemButtonProps } from '@sienar/react-components/src/List/ListItemButton';

type Props = DrawerItem & Omit<ListItemButtonProps, 'color' | 'variant'>

export default function DashboardMenuItem(props: Props) {
	const {
		text,
		icon,
		endIcon,
		authorization,
		any
	} = props;
	const isAuthorized = useDrawerItemAuthorized(authorization, any);

	// Don't clean href or to, as these are used to decide whether to use a <NavLink>, <a>, or <button>
	const cleanedProps = cleanProps(props, 'text', 'icon', 'endIcon', 'children', 'authorization', 'any', 'priority');
	const output = (
		<List.ItemButton {...cleanedProps}>
			<List.ItemIcon position='start'>
				{icon}
			</List.ItemIcon>
			<List.ItemText>{text}</List.ItemText>
			<List.ItemIcon position='end'>
				{endIcon}
			</List.ItemIcon>
		</List.ItemButton>
	);

	return isAuthorized && output || <></>;
}