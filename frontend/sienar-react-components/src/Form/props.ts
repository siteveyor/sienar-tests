import type { LegacyRef, PropsWithChildren } from 'react';
import type {FormValueValidator} from '@/types';

interface FormInputPropsBase<T extends unknown> {
	validators?: FormValueValidator<T>[]
	value: T
	setValue: (input: T) => void
	inputRef?: LegacyRef<HTMLInputElement> | undefined
}

export interface FormInputProps<T extends unknown> extends PropsWithChildren<FormInputPropsBase<T>> {}