import {useState} from 'react';
import { Collapse, Icon, List } from '@sienar/react-components';
import {DrawerItem} from '@/utils/types';
import DashboardMenuItem from '@/components/dashboard/DashboardMenuItem';
import {useDrawerItemAuthorized} from '@/utils/hooks';
import { ButtonProps } from '@sienar/react-components/src/Button/Button';

type Props = DrawerItem & Omit<ButtonProps, 'color' | 'variant'|'children'>

export default function DashboardMenuGroup(props: Props) {
	const {
		text,
		icon,
		authorization,
		any,
		children
	} = props;
	const [open, setOpen] = useState(false);
	const isAuthorized = useDrawerItemAuthorized(authorization, any);

	if (!isAuthorized) {
		return <></>;
	}

	const toggleOpen = () => setOpen(!open);

	const endIconString = open ? 'chevron-up' : 'chevron-down';
	const endIcon = <Icon icon={endIconString}/>;

	const nestedList = (
		<Collapse open={open}>
			<List type='nav'>
				{children?.map(child => child.children
					? (
						<DashboardMenuGroup
							key={child.text}
							text={child.text}
							icon={child.icon}
							authorization={child.authorization}
							any={child.any}
							children={child.children}
						/>
					) : (
						<DashboardMenuItem
							key={child.text}
							text={child.text}
							icon={child.icon}
							endIcon={child.endIcon}
							authorization={child.authorization}
							href={child.href}
							to={child.to}
						/>
					)
				)}
			</List>
		</Collapse>
	);

	return (
		<DashboardMenuItem
			text={text}
			icon={icon}
			endIcon={endIcon}
			authorization={authorization}
			any={any}
			onClick={toggleOpen}
			nestedList={nestedList}
		/>
	);
}