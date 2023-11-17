import { useFormField } from './utils';
import ErrorList from './ErrorList';

import type { ChangeEvent } from 'react';
import type { FormInputProps } from './props';

interface TextInputProps<T extends string | number> extends FormInputProps<T> {
	type?: 'text' | 'password';
	serverErrors?: string[]
	setServerErrors?: (newErrors: string[]) => void
}

export default function TextInput<T extends string | number>(props: TextInputProps<T>) {
	const {
		validators = [],
		value,
		setValue,
		type = 'text',
		children,
		inputRef,
		serverErrors = [],
		setServerErrors
	} = props;

	const [
		fieldId,
		errors,
		interact,
		classNames
	] = useFormField(value, 'form-text-field', validators);

	const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		const newValue = (typeof value === 'number'
			? parseFloat(e.target.value)
			: e.target.value) as T;
		if (value !== newValue) {
			interact();
			setValue(newValue);
			setServerErrors?.([]);
		}
	}

	return (
		<div className={classNames.componentWrapper}>
			<div className={classNames.labelWrapper}>
				<label
					className={classNames.label}
					htmlFor={fieldId.current}
				>
					{children}
				</label>
			</div>

			<div className={classNames.inputWrapper}>
				<input
					id={fieldId.current}
					className={classNames.input}
					value={value}
					onChange={handleChange}
					type={type}
					ref={inputRef}
				/>
			</div>

			<ErrorList errors={[...errors, ...serverErrors]}/>
		</div>
	);
};