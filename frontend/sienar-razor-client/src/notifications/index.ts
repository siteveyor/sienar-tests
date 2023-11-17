import { Toast } from 'bootstrap';

import type { ToastDto } from '@/notifications/renderUtils';
import { generateToastMarkup, MessageType } from '@/notifications/renderUtils';

const toastContainer = document.getElementById('sienar-toast-container');

/**
 * Generates a Toast notification
 * 
 * @param {ToastDto} toast The notification data to display
 */
export function dispatchToast(toast: ToastDto) {
	if (!toastContainer) {
		return;
	}
	toast.delay ??= 5000;

	const toastElement = generateToastMarkup(toast);
	toastContainer.appendChild(toastElement);
	const toastInstance = Toast.getOrCreateInstance(toastElement, {
		delay: toast.delay,
		autohide: toast.delay > 0
	});
	toastInstance.show();
}

export function dispatchPrimaryToast(bodyText: string, titleText?: string) {
	dispatchToast({bodyText, titleText, type: MessageType.Default, isBackgroundTheme: true});
}

export function dispatchSuccessToast(bodyText: string, titleText?: string) {
	dispatchToast({bodyText, titleText, type: MessageType.Success, isBackgroundTheme: true});
}

export function dispatchErrorToast(bodyText: string, titleText?: string) {
	dispatchToast({bodyText, titleText, type: MessageType.Error, isBackgroundTheme: true, delay: 0});
}

export function dispatchWarningToast(bodyText: string, titleText?: string) {
	dispatchToast({bodyText, titleText, type: MessageType.Warning, isBackgroundTheme: true, delay: 0});
}

export function dispatchInfoToast(bodyText: string, titleText?: string) {
	dispatchToast({bodyText, titleText, type: MessageType.Info, isBackgroundTheme: true});
}

export function dispatchPlainToast(bodyText: string, titleText?: string) {
	dispatchToast({bodyText, titleText, type: MessageType.None, isBackgroundTheme: true});
}