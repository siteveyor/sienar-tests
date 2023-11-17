import {fileURLToPath, URL} from 'node:url';
import {defineConfig} from 'vite';
import react from '@vitejs/plugin-react';
import dts from 'vite-plugin-dts';
const libname = 'sienar-react-components';

const external = [
	'react',
	'react/jsx-runtime',
	'react-router-dom',
	'react-dom'
];
export default defineConfig({
	build: {
		lib: {
			entry: 'src/index.ts',
			name: libname,
			fileName: libname
		},
		rollupOptions: {
			external,
			// output: {
			// 	globals: {
			// 		'react': 'React',
			// 		'react/jsx-runtime': 'JSXRuntime',
			// 		'react-router-dom': 'ReactRouterDOM'
			// 	}
			// }
		}
	},
	plugins: [
		react(),
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