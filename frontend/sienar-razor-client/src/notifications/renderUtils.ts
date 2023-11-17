export enum MessageType {
	Default,
	Success,
	Error,
	Warning,
	Info,
	None
}

export type ToastDto = {
	type: MessageType
	isBackgroundTheme: boolean
	titleText?: string
	bodyText: string
	delay?: number
}

export function generateToastMarkup(toast: ToastDto): HTMLDivElement {
	const el = document.createElement('div');
	el.className = createToastWrapperClasses(toast);

	if (toastHasHeader(toast)) {
		el.appendChild(generateToastHeader(toast));
	}

	el.appendChild(generateToastBody(toast));

	return el;
}

function generateToastHeader(toast: ToastDto): HTMLDivElement {
	const el = document.createElement('div');
	el.className = `toast-header ${createToastThemeClass(toast)}`;
	el.innerHTML = `
		<strong class="me-auto">${toast.titleText}</strong>
		<button class="btn-close btn-close-dark"
				data-bs-dismiss="toast"
				type="button">
		</button>
	`;

	return el;
}

function generateToastBody(toast: ToastDto): HTMLDivElement {
	const el = document.createElement('div');
	el.className = createToastBodyClasses(toast);

	const body = document.createElement('div');
	body.className = 'toast-body';
	body.innerText = toast.bodyText;
	el.appendChild(body);

	if (!toastHasHeader(toast)) {
		const closeButton = document.createElement('button');
		closeButton.className = 'btn-close btn-close-dark me-2 m-auto';
		closeButton.type = 'button';
		closeButton.dataset['bsDismiss'] = 'toast';
		el.appendChild(closeButton);
	}

	return el;
}

function createToastWrapperClasses(toast: ToastDto): string {
	return !toastHasHeader(toast) && toast.isBackgroundTheme && toast.type != MessageType.None
		? 'toast border-0'
		: 'toast';
}

function createToastThemeVariant(type: MessageType): string {
	switch(type) {
		case MessageType.Default:
			return 'primary';
		case MessageType.None:
			return '';
		case MessageType.Error:
			return 'danger';
		default:
			return MessageType[type].toLowerCase();
	}
}

function createToastThemeClass(toast: ToastDto): string {
	if (toastHasThemeVariant(toast)) {
		const variant = createToastThemeVariant(toast.type);
		return toast.isBackgroundTheme
			? `text-bg-${variant}`
			: `text-${variant}`;
	}

	return '';
}

function createToastBodyClasses(toast: ToastDto): string {
	return toastHasHeader(toast) || !toastHasThemeVariant(toast)
		? 'd-flex'
		: `d-flex ${createToastThemeClass(toast)}`;
}

function toastHasHeader(toast: ToastDto): boolean {
	return !!toast.titleText;
}

function toastHasThemeVariant(toast: ToastDto): boolean {
	return toast.type !== MessageType.None;
}