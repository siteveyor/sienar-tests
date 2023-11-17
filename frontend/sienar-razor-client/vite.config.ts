import {fileURLToPath, URL} from 'node:url';
import {defineConfig} from 'vite';
import dts from 'vite-plugin-dts';
const libname = 'sienar-razor-client';

const external = [
	'bootstrap'
];

export default defineConfig({
	build: {
		lib: {
			entry: 'src/index.ts',
			name: libname,
			fileName: libname
		},
		rollupOptions: {
			external
		}
	},
	plugins: [
		dts({ rollupTypes: true })
	],
	optimizeDeps: {
		exclude: external
	},
	resolve: {
		alias: {
			'@': fileURLToPath(new URL('./src', import.meta.url))
		}
	}
});