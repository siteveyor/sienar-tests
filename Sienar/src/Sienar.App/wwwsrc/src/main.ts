import type { Sienar } from '@sienar/razor-client';

declare global {
	interface Window {
		sienar: Sienar
	}
}

import 'bootstrap';
import '@/tools/table-form';
import '@/tools/render-server-notifications';