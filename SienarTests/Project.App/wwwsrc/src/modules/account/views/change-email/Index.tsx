import { Form } from '@sienar/react-components';
import NarrowContainer from '@/components/ui/NarrowContainer';
import StandardForm from '@/components/ui/forms/StandardForm';
import {useNavigate} from 'react-router';
import {useState} from 'react';
import {initiateEmailChange} from '@account/services';
import links from '@account/links';
import {isEmail, matches, required} from '@/utils/validators';
import type {InitiateEmailChangeDto} from '@account/services';

export default function Index() {
	const navigate = useNavigate();
	const [email, setEmail] = useState('');
	const [confirmEmail, setConfirmEmail] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');

	const doEmailChange = async () => {
		const data: InitiateEmailChangeDto = {
			email,
			confirmEmail,
			confirmPassword
		};

		const wasSuccessful = await initiateEmailChange(data);
		if (wasSuccessful) {
			navigate(links.changeEmail.requested);
		}

		return wasSuccessful;
	};

	return (
		<NarrowContainer>
			<StandardForm
				title='Change email'
				onSubmit={doEmailChange}
				submitText='Update email'
			>
				<Form.TextInput
					value={email}
					setValue={setEmail}
					validators={[
						required('new email'),
						isEmail('new email')
					]}
				>
					Enter your new email address
				</Form.TextInput>
				<Form.TextInput
					value={confirmEmail}
					setValue={setConfirmEmail}
					validators={[matches(email, 'Your emails do not match')]}
				>
					Confirm your new email address
				</Form.TextInput>
				<Form.TextInput
					value={confirmPassword}
					setValue={setConfirmPassword}
					type='password'
					validators={[required('confirm password')]}
				>
					Confirm your password
				</Form.TextInput>
			</StandardForm>
		</NarrowContainer>
	);
}