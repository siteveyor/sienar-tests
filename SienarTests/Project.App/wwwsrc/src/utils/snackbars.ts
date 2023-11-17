// import {enqueueSnackbar, SnackbarOrigin} from 'notistack';

const anchorOrigin = {
	horizontal: 'right',
	vertical: 'top'
};

export function createSuccessSnackbar(message: string) {
	enqueueSnackbar(message, {
		variant: 'success',
		anchorOrigin
	});
}

export function createWarningSnackbar(message: string) {
	enqueueSnackbar(message, {
		variant: 'warning',
		anchorOrigin
	});
}

export function createErrorSnackbar(message: string) {
	enqueueSnackbar(message, {
		persist: true,
		variant: 'error',
		anchorOrigin
	});
}

export function createInfoSnackbar(message: string) {
	enqueueSnackbar(message, {
		variant: 'info',
		anchorOrigin
	});
}

function enqueueSnackbar(message: string, config: Object) {
	console.log('snackbar:', message);
}