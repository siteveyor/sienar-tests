import {FormValueValidator} from '@/utils/types';

export function required(fieldName: string): FormValueValidator<any> {
	return (input: any) => {
		// Damn you, JavaScript. You and your "falsiness"
		if (input === 0) {
			return null;
		}

		// Bless you JavaScript. You and your "falsiness"
		if (!input) {
			return `${capitalize(fieldName)} is required.`;
		}

		return null;
	}
}

export function minLength(fieldName: string, min: number): FormValueValidator<string> {
	return (input: string) => {
		if (!input || input.length >= min) {
			return null;
		}

		return `${capitalize(fieldName)} must be at least ${min} characters.`;
	}
}

export function maxLength(fieldName: string, max: number): FormValueValidator<string> {
	return (input: string) => {
		if (!input || input.length <= max) {
			return null;
		}

		return `${capitalize(fieldName)} must be shorter than ${max} characters.`
	}
}

export function matches(other: string, message: string): FormValueValidator<string> {
	return (input: string) => {
		if (!input || input === other) {
			return null;
		}

		return message;
	}
}

export function isEmail(fieldName: string): FormValueValidator<string> {
	return (input: string) => {
		if (!input || /^\S+@\S+\.\S+$/.test(input)) {
			return null;
		}

		return `${capitalize(fieldName)} must be a valid email address.`;
	}
}

export function matchesRegex(test: RegExp, message: string) :FormValueValidator<string> {
	return (input: string) => {
		if (!input || test.test(input)) {
			return null;
		}

		return message;
	}
}

export function containsSpecialCharacter(fieldName: string): FormValueValidator<string> {
	return matchesRegex(/[\W_]/, `${capitalize(fieldName)} must contain at least one special character.`);
}

export function containsNumber(fieldName: string): FormValueValidator<string> {
	return matchesRegex(/\d/, `${capitalize(fieldName)} must contain at least one number.`);
}

export function containsUpper(fieldName: string): FormValueValidator<string> {
	return matchesRegex(/[A-Z]/, `${capitalize(fieldName)} must contain at least one uppercase letter.`);
}
export function containsLower(fieldName: string): FormValueValidator<string> {
	return matchesRegex(/[a-z]/, `${capitalize(fieldName)} must contain at least one lowercase letter.`);
}

function capitalize(input: string): string {
	return input.substring(0, 1).toUpperCase() + input.substring(1);
}