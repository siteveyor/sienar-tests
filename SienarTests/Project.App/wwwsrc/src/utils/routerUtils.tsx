import {RouteObject} from 'react-router';
import Layout from '@/components/coreViews/_Layout';
import AuthorizeRoute from '@/components/auth/AuthorizeRoute';
import Dashboard from '@/components/coreViews/Dashboard';
import Unauthorized from '@/components/coreViews/Unauthorized';
import NotFound from '@/components/coreViews/NotFound';
import React from 'react';
import {createBrowserRouter} from 'react-router-dom';
import {Module} from '@/utils/types';

export function createRouter(...modules: Module[]) {
	const drawerItems = modules
		.filter(m => m.drawerItems)
		.flatMap(m => m.drawerItems!);

	const route: RouteObject = {
		path: '/',
		element: <Layout drawerItems={drawerItems}/>,
		children: [
			{
				path: '',
				element: (
					<AuthorizeRoute>
						<Dashboard/>
					</AuthorizeRoute>
				)
			},
			{
				path: 'unauthorized',
				element: <Unauthorized/>
			},
			...modules.map(m => m.router),
			{
				path: '*',
				element: <NotFound/>
			}
		]
	};

	return createBrowserRouter([route], {basename: '/dashboard'});
}

export function createCrudRouter(
	basePath: string,
	indexElement: JSX.Element,
	addElement: JSX.Element,
	editElement: JSX.Element
): RouteObject {
	return {
		path: basePath,
		children: [
			{
				path: '',
				element: indexElement
			},
			{
				path: 'add',
				element: addElement
			},
			{
				path: 'edit/:id',
				element: editElement
			}
		]
	};
}