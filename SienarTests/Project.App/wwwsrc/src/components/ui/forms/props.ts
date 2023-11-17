import {FormValueValidator} from '@/utils/types';
import {PropsWithChildren, RefObject} from 'react';

interface FormInputPropsBase<T extends unknown> {
	validators?: FormValueValidator<T>[]
	value: T
	setValue: (input: T) => void
	inputRef?: ((instance: (HTMLDivElement | null)) => void) | RefObject<HTMLDivElement> | null | undefined
}

export interface FormInputProps<T extends unknown> extends PropsWithChildren<FormInputPropsBase<T>> {}