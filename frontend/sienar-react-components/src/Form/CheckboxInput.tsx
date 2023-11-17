import { useFormField } from './utils';
import ErrorList from './ErrorList';

import type { ChangeEvent } from 'react';
import type { FormInputProps } from './props';

export default function CheckboxInput(props: FormInputProps<boolean>) {
	const {
		validators = [],
		value,
		setValue,
		children
	} = props;

	const [
		fieldId,
		errors,
		interact,
		classNames
	] = useFormField(value, 'form-check-field', validators);

	const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
		const newValue = e.target.checked;
		if (value !== newValue) {
			setValue(e.target.checked);
			interact();
		}
	}

	return (
		<div className={classNames.componentWrapper}>
			<div className={classNames.inputWrapper}>
				<input
					id={fieldId.current}
					className={classNames.input}
					checked={value}
					onChange={handleChange}
					type='checkbox'
				/>
			</div>

			<div className={classNames.labelWrapper}>
				<label
					className={classNames.label}
					htmlFor={fieldId.current}
				>
					{children}
				</label>
			</div>

			<ErrorList errors={errors}/>
		</div>
	);
}