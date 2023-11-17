import {containsLower, containsNumber, containsSpecialCharacter, containsUpper, minLength, required} from '@/utils/validators';
import {FormValueValidator} from '@/utils/types';

export function passwordValidators(fieldName: string): FormValueValidator<string>[] {
	return [
		required(fieldName),
		minLength(fieldName, 8),
		containsSpecialCharacter(fieldName),
		containsNumber(fieldName),
		containsUpper(fieldName),
		containsLower(fieldName)
	];
}