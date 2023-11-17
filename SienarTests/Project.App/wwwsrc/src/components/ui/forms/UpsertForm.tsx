import StandardForm from '@/components/ui/forms/StandardForm';
import {PropsWithChildren, ReactNode, useEffect, useRef} from 'react';
import {useNavigate, useParams} from 'react-router';
import {CrudService} from '@/utils/services';
import {EntityBase} from '@/utils/types';

interface Props<T extends EntityBase> {
	mapDto: () => T
	populateData: (data: T) => Promise<void>|void
	resetForm: () => void
	service: CrudService<T>
	addTitle: string
	addSubmitText: string
	editTitle: string
	editSubmitText: string,
	successLink: string,
	addMessage?: ReactNode
	editMessage?: ReactNode
	additionalActions?: ReactNode
}

export default function UpsertForm<T extends EntityBase>(props: PropsWithChildren<Props<T>>) {
	const {
		children,
		mapDto,
		populateData,
		service,
		resetForm,
		addTitle,
		addSubmitText,
		editTitle,
		editSubmitText,
		successLink,
		addMessage,
		editMessage,
		additionalActions
	} = props;

	const navigate = useNavigate();
	const params = useParams();
	const id = useRef<string>(params['id'] ?? '');
	const isEditing = !!id.current;

	useEffect(() => {
		(async function() {
			if (!isEditing) {
				return;
			}

			const existing = await service.getById(id.current)
			if (!existing) {
				return;
			}

			await populateData(existing);
		})();
	}, [])

	const onSubmit = async (): Promise<boolean> => {
		const data = mapDto();
		let func: (data: T) => Promise<string|boolean>;
		if (isEditing) {
			data.id = id.current;
			func = service.edit;
		} else {
			func = service.add;
		}

		const wasSuccessful = await func(data);
		if (wasSuccessful) {
			if (isEditing) {
				navigate(successLink);
			} else {
				resetForm();
			}
		}

		return !!wasSuccessful;
	};

	return (
		<StandardForm
			title={isEditing ? editTitle : addTitle}
			submitText={isEditing ? editSubmitText : addSubmitText}
			onSubmit={onSubmit}
			information={isEditing ? editMessage : addMessage}
			additionalActions={additionalActions}
		>
			{children}
		</StandardForm>
	)
}