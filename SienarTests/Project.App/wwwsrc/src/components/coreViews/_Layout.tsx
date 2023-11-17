import {Outlet} from 'react-router-dom';
import { AppBar, Button, Drawer } from '@sienar/react-components';
// import { Box, Toolbar} from '@mui/material';
import {useState} from "react";
import {DrawerItem} from '@/utils/types';
import DashboardDrawerContent from '@/components/dashboard/DashboardDrawerContent';

interface Props {
	drawerItems: DrawerItem[]
}

export default function Layout({drawerItems}: Props) {
	const [open, setOpen] = useState(false);

	// const drawerWidth = '20%';
	// const drawerMinWidth = '200px';
	// const drawerMaxWidth = '300px';

	// const drawerCommonStyles = {
	// 	display: 'flex',
	// 	minHeight: '100vh',
	// 	flexDirection: 'column'
	// };

	return (
		// <Box sx={{display: 'flex'}}>
		<>
			<AppBar fixed>
				<Button.Icon
					className='app-bar__toggler'
					size='lg'
					icon='bars'
					onClick={() => setOpen(!open)}
					suppressTheme
				/>
				{/*<Toolbar>*/}
					<h6>#sitename#</h6>
				{/*</Toolbar>*/}
			</AppBar>

			<Drawer
				permanent={true} // TODO: Change this to use breakpoint provider when done
				anchor='left'
				open={open}
				onClose={() => setOpen(false)}
			>
				<DashboardDrawerContent drawerItems={drawerItems}/>
			</Drawer>

			<div
				// sx={{
				// 	flexGrow: 1,
				// 	display: 'flex',
				// 	flexDirection: 'column',
				// 	minHeight: '100vh'
				// }}
			>
				<div
					// sx={{ flexGrow: 1 }}
				>
					<main
						// sx={{p:4}}
					>
						<Outlet/>
					</main>
				</div>

				<footer>
					&copy;{new Date().getFullYear()} #sitename#. All rights reserved.
				</footer>
			</div>
		{/*</Box>*/}
		</>
	);
}