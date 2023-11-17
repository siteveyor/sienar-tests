import {fileURLToPath, URL} from 'node:url';
import {defineConfig} from 'vite';

const external = [
	'bootstrap',
	'@sienar/razor-client'
];

// noinspection JSUnusedGlobalSymbols
export default defineConfig({
	build: {
		rollupOptions: {
			input: {
				'main': 'src/main.ts'
			},
			output: {
				dir: '../wwwroot',
				entryFileNames: '[name].js'
			},
			external
		}
	},
	optimizeDeps: {
		exclude: external
	},
	resolve: {
		alias: {
			'@': fileURLToPath(new URL('./src', import.meta.url))
		}
	}
});