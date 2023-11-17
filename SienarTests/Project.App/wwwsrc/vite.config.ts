import {fileURLToPath, URL} from 'node:url';

import {defineConfig} from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
// noinspection JSUnusedGlobalSymbols
export default defineConfig({
	base: '/dashboard',
	build: {
		outDir: '../wwwroot/dashboard'
	},
	plugins: [react()],
	resolve: {
		alias: {
			'@': createPath('./src'),
			'@account': createPath('./src/modules/account'),
			'@states': createPath('./src/modules/states')
		}
	},
	server: {
		port: 8080,
		proxy: {
			'/sienar': {
				target: 'http://localhost:5000',
				changeOrigin: true
			}
		}
	}
});

function createPath(moduleBase: string): string {
	return fileURLToPath(new URL(moduleBase, import.meta.url));
}