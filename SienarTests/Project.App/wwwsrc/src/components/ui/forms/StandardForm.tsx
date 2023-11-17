import StandardCard from '@/components/ui/StandardCard';
import {createContext, MouseEvent, PropsWithChildren, ReactNode, useContext, useEffect, useRef} from 'react';
import {Button} from '@sienar/react-components';
import {FormFieldValidator} from "@/utils/types";

interface Props {
	title: string
	onSubmit: () => Promise<boolean>
	onReset?: () => Promise<void> | void
	submitText?: string
	resetText?: string
	showReset?: boolean
	information?: ReactNode
	additionalActions?: ReactNode
}

interface Context {
	hasInteracted: boolean
	validators: Record<string, FormFieldValidator>
}

export const formErrorContext = createContext<Context>({
	hasInteracted: false,
	validators: {}
});

export default function StandardForm(props: PropsWithChildren<Props>) {
	const {
		title,
		onSubmit,
		onReset,
		submitText = 'Submit',
		resetText = 'Reset',
		showReset = false,
		information,
		additionalActions,
		children
	} = props;

	const errorContext = useContext(formErrorContext);
	const formId = useRef(`form-${Math.random().toString().substring(2)}`);

	const handleSubmit = async (e: MouseEvent<HTMLButtonElement>) => {
		e.preventDefault();
		errorContext.hasInteracted = true;

		let valid = true;
		for (let validator in errorContext.validators) {
			if (!errorContext.validators[validator]()) {
				valid = false;
			}
		}

		if (valid && await onSubmit()) {
			await doReset();
		}
	};

	const handleReset = async (e: MouseEvent<HTMLButtonElement>) => {
		e.preventDefault();
		await doReset();
	}

	const doReset = async () => {
		errorContext.hasInteracted = false;
		await onReset?.();
	}

	// This effect does nothing on load, but when the component unmounts,
	// it resets hasInteracted. Without this, revisiting a submitted form
	// may cause validation to run on load, because
	// a) React caches components for reuse
	// b) the existing errorContext.hasInteracted will therefore be true
	useEffect(() => {
		return () => {
			errorContext.hasInteracted = false;
		};
	}, []);

	const actions = (
		<>
			<Button
				onClick={handleSubmit}
				form={formId.current}
				type='submit'
			>
				{submitText}
			</Button>

			{showReset && (
				<Button
					onClick={handleReset}
					color='secondary'
					type='reset'
				>
					{resetText}
				</Button>
			)}
			{additionalActions}
		</>
	);

	return (
		<formErrorContext.Provider value={errorContext}>
			<StandardCard
				title={title}
				actions={actions}
			>
				{information}

				<form id={formId.current}>
					{children}
				</form>
			</StandardCard>
		</formErrorContext.Provider>
	);
}