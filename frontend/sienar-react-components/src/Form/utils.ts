import { createContext, useContext, useEffect, useRef, useState } from 'react';
import type { MutableRefObject } from 'react';
import type { FormFieldValidator, FormValueValidator } from '@/types';

interface Context {
	hasInteracted: boolean
	validators: Record<string, FormFieldValidator>
}

interface ClassNames {
	componentWrapper?: string
	labelWrapper?: string
	label?: string
	inputWrapper?: string
	input?: string

}

export const formErrorContext = createContext<Context>({
	hasInteracted: false,
	validators: {}
});

export function useFormField<T extends unknown>(
	value: T,
	baseClassName: string,
	validators: FormValueValidator<T>[]
): [
	MutableRefObject<string>,
	string[],
	() => void,
	ClassNames
] {
	const id = useRef(`input-${Math.random().toString().substring(2)}`);
	const [errors, setErrors] = useState<string[]>([]);
	const errorContext = useContext(formErrorContext);

	const validate: FormFieldValidator = () => {
		if (!errorContext.hasInteracted) {
			return false;
		}

		const internalErrors: string[] = [];

		for (let validator of validators) {
			const error = validator(value);
			if (error) {
				internalErrors.push(error);
			}
		}

		setErrors(internalErrors);
		return internalErrors.length === 0;
	}

	useEffect(() => {
		errorContext.validators[id.current] = validate;
		validate();

		// Mostly, this isn't an issue
		// but if the component re-mounts,
		// extra validators will be hanging out
		// which is no bueno
		return () => {delete errorContext.validators[id.current]};
	}, [value]);

	const classNames: ClassNames = createClassNames(
		baseClassName,
		errorContext.hasInteracted,
		errors.length > 0);

	return [id, errors, () => errorContext.hasInteracted = true, classNames];
}

function createClassNames(baseClassName: string, hasInteracted: boolean, hasErrors: boolean): ClassNames {
	return {
		componentWrapper: createStatusAwareClassName(baseClassName, hasInteracted, hasErrors),
		labelWrapper: createStatusAwareClassName(`${baseClassName}__label-wrapper`, hasInteracted, hasErrors),
		label: createStatusAwareClassName(`${baseClassName}__label`, hasInteracted, hasErrors),
		inputWrapper: createStatusAwareClassName(`${baseClassName}__input-wrapper`, hasInteracted, hasErrors),
		input: createStatusAwareClassName(`${baseClassName}__input`, hasInteracted, hasErrors),
	}
}

function createStatusAwareClassName(baseClassName: string, hasInteracted: boolean, hasErrors: boolean): string {
	if (!hasInteracted) {
		return baseClassName;
	}

	let className = baseClassName;
	if (hasErrors) {
		className = `${className} ${className}--error`;
	} else {
		className = `${className} ${className}--success`;
	}

	return className;
}