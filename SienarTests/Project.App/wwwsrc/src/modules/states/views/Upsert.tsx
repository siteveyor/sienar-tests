import { Form } from '@sienar/react-components';
import {useRef, useState} from 'react';
import NarrowContainer from '@/components/ui/NarrowContainer';
import {maxLength, minLength, required} from '@/utils/validators';
import {Button} from '@sienar/react-components';
import links from '../links';
import UpsertForm from '@/components/ui/forms/UpsertForm';
import {stateService} from '@states//services';
import type { StateDto } from '@states/services';

export default function Upsert() {
	const [name, setName] = useState('');
	const [abbreviation, setAbbreviation] = useState('');
	const nameInput = useRef<HTMLInputElement>(null);

	const mapDto = () => {
		return {
			name,
			abbreviation
		} as StateDto;
	};

	const populateData = (existing: StateDto) => {
		setName(existing.name);
		setAbbreviation(existing.abbreviation);
	};

	const reset = () => {
		setName('');
		setAbbreviation('');
		nameInput.current!.focus();
	};

	const backToStatesListing = (
		<Button
			to={links.index}
			color='secondary'
		>
			Return to states listing
		</Button>
	);

	return (
		<NarrowContainer>
			<UpsertForm
				mapDto={mapDto}
				populateData={populateData}
				service={stateService}
				resetForm={reset}
				addTitle='Add state'
				addSubmitText='Add state'
				editTitle='Edit state'
				editSubmitText='Update state'
				successLink={links.index}
				additionalActions={backToStatesListing}
			>
				<Form.TextInput
					value={name}
					setValue={setName}
					validators={[required('name')]}
					inputRef={nameInput}
				>
					State name
				</Form.TextInput>
				<Form.TextInput
					value={abbreviation}
					setValue={setAbbreviation}
					validators={[
						required('abbreviation'),
						minLength('abbreviation', 2),
						maxLength('abbreviation', 2)
					]}
				>
					State abbreviation
				</Form.TextInput>
			</UpsertForm>
		</NarrowContainer>
	)
}