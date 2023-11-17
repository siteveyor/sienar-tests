const base = '/account';
const adminBase = '/users';

export default {
	admin: {
		index: adminBase,
		add: `${adminBase}/add`,
		edit: `${adminBase}/edit`
	},
	deleted: `${base}/deleted`,
	login: `${base}/login`,
	personalData: {
		index: `${base}/personal-data`,
		download: `${base}/personal-data`
	},
	changeEmail: {
		index: `${base}/change-email`,
		confirm: `${base}/change-email/confirm`,
		requested: `${base}/change-email/requested`,
		successful: `${base}/change-email/successful`
	},
	changePassword: {
		index: `${base}/change-password`,
		successful: `${base}/change-password/successful`
	},
	confirm: {
		index: `${base}/confirm`,
		successful: `${base}/confirm/successful`
	},
	forgotPassword: {
		index: `${base}/forgot-password`,
		successful: `${base}/forgot-password/successful`
	},
	register: {
		index: `${base}/register`,
		successful: `${base}/register/successful`
	},
	resetPassword: {
		index: `${base}/reset-password`,
		successful: `${base}/reset-password/successful`
	}
};