import { Button, Icon, List } from '@sienar/react-components';
import DashboardMenuItem from '@/components/dashboard/DashboardMenuItem';
import DashboardMenuGroup from '@/components/dashboard/DashboardMenuGroup';
import accountLinks from '@account/links';
import {DrawerItem} from '@/utils/types';
import {useAuthorized} from '@/utils/hooks';
import {useState} from 'react';
import {logout} from "@account/services";
import {useNavigate} from "react-router";
import links from "@account/links";

interface Props {
	drawerItems: DrawerItem[]
}

export default function DashboardDrawerContent({drawerItems}: Props) {
	const navigate = useNavigate();
	const isLoggedIn = useAuthorized();
	const [isLoggingOut, setIsLoggingOut] = useState(false);
	const loginButtonLink = accountLinks.login;
	const loginButtonText = isLoggedIn ? (isLoggingOut ? 'Logging out...' : 'Log out') : 'Log in';

	async function onLogOutClicked(): Promise<void> {
		if (isLoggingOut || !isLoggedIn) {
			return;
		}

		setIsLoggingOut(true);

		if (await logout()) {
			navigate(links.login);
			setIsLoggingOut(false);
		}
	}

	drawerItems.forEach(d => d.priority ??= 10);

	drawerItems.sort((a, b) => {
		const aPriority = a.priority!;
		const bPriority = b.priority!;

		if (aPriority < bPriority) {
			return -1;
		}

		if (bPriority < aPriority) {
			return 1;
		}

		return 0;
	})

	return (
		<div
			// sx={{
			// 	height: '100%',
			// 	display: 'flex',
			// 	flexDirection: 'column',
			// 	justifyContent: 'space-between'
			// }}
		>
			<div>
				<List type='nav'>
					<DashboardMenuItem
						text='Dashboard'
						to='/'
						icon={<Icon icon='box'/>}
						authorization
					/>
					{drawerItems.map(d => d.children
						? <DashboardMenuGroup
							text={d.text}
							icon={d.icon}
							authorization={d.authorization}
							any={d.any}
							children={d.children}
							key={d.text}
						/>
						: <DashboardMenuItem
							text={d.text}
							icon={d.icon}
							endIcon={d.endIcon}
							authorization={d.authorization}
							any={d.any}
							href={d.href}
							to={d.to}
							key={d.text}
						/>
					)}
					<DashboardMenuItem
						text='Return home'
						href='/'
						icon={<Icon icon='home'/>}
					/>
				</List>
			</div>

			<div
			// 	sx={{
			// 	width: '100%',
			// 	p: 2
			// }}
			>
				{!isLoggedIn && <>
					<Button
						// sx={{
						// 	width: '100%',
						// 	mb: 2
						// }}
						color='secondary'
						variant='outlined'
						to={accountLinks.register.index}
					>
						Register
					</Button>
					<Button
						// sx={{ width: '100%' }}
						to={loginButtonLink}
					>
						{loginButtonText}
					</Button>
				</>}

				{isLoggedIn && <Button
					// sx={{ width: '100%'}}
					onClick={onLogOutClicked}
					>
					{loginButtonText}
				</Button>}
			</div>
		</div>
	);
}