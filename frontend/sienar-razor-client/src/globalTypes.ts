import type { ToastDto } from '@/notifications/renderUtils';

export type Sienar = {
	notifications: ToastDto[]
}

declare global {
	interface Window { sienar: Sienar }
}