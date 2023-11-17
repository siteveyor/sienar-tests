import {MutableRefObject, useContext, useEffect, useRef, useState} from 'react';
import {formErrorContext} from "@/components/ui/forms/StandardForm";
import {DrawerItem, FormFieldValidator, FormValueValidator} from '@/utils/types';
import {RootState} from '@/store';
import {useSelector} from 'react-redux';

export function useFormField<T extends unknown>(
    value: T,
    validators: FormValueValidator<T>[]
): [
	MutableRefObject<string>,
	string[],
	() => void
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

    return [id, errors, () => errorContext.hasInteracted = true];
}

export function useAuthorized(
	roles: string|string[]|null = null,
	any: boolean|null = false
): boolean {
	const isLoggedIn = useSelector((state: RootState) => state.appData.isLoggedIn);
	const user = useSelector((state: RootState) => state.appData.user);

	if (!roles) {
		return isLoggedIn;
	}

	if (!isLoggedIn || !user) {
		return false;
	}

	const userRoles = user.roles.map(r => r.name);

	if (typeof roles === 'string') {
		return userRoles.includes(roles);
	}

	if (Array.isArray(roles)) {
		for (let r of roles) {
			const found = userRoles.includes(r);

			// If we found the role and any role will do, the user is authorized
			if (found && any) {
				return true;
			}
			// If we didn't find the role and all props are required, the user isn't authorized
			else if (!found && !any) {
				return false;
			}
		}

		// If we got here, either no roles were found when any role will do,
		// or all roles were found when all roles were required
		// so the result is the opposite of whether the any prop is set
		return !any;
	}

	// Shouldn't ever get here...famous last words
	return false;
}

export function useDrawerItemAuthorized(
	authorization: string | string[] | boolean | undefined,
	any: boolean | undefined = false
): boolean {
	const automaticallyAuthorized = authorization === false
		|| typeof authorization === 'undefined';
	const roles = typeof authorization === 'boolean'
		? undefined
		: authorization;
	const isAuthorized = useAuthorized(roles, any);

	return automaticallyAuthorized || isAuthorized;
}