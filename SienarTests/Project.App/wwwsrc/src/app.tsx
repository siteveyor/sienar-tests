import '@/componentTheme/main.scss';
import '@/theme/main.scss';

import React from 'react';
import ReactDOM from 'react-dom/client';
import {RouterProvider} from 'react-router-dom';
import {Provider} from 'react-redux';
import {store} from '@/store';
import {startSession} from '@account/services';
import {createRouter} from '@/utils/routerUtils';

// Import modules
import accountModule from '@account/index';
import statesModule from '@states/index';

// Setup
const router = createRouter(
	accountModule,
	statesModule
);

(async function() {
	await startSession();

	ReactDOM.createRoot(document.getElementById('root')!)
		.render(
			<React.StrictMode>
				<Provider store={store}>
					<RouterProvider router={router}/>
				</Provider>
			</React.StrictMode>
		);
})();
