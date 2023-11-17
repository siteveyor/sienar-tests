import {useNavigate} from 'react-router';
import {useRef, useState} from 'react';
import { Form } from '@sienar/react-components';
import {requestPasswordReset} from '@account/services';
import links from '@account/links';
import StandardForm from '@/components/ui/forms/StandardForm';
import NarrowContainer from '@/components/ui/NarrowContainer';
import {required} from '@/utils/validators';
import type {RequestPasswordResetDto} from '@account/services';

export default function Index() {
	const navigate = useNavigate();
	const [accountName, setAccountName] = useState('');
	const [secretKeyValue, setSecretKeyValue] = useState('');
	const timeToComplete = useRef(Date.now());

	const doPasswordReset = async () => {
		const data: RequestPasswordResetDto = {
			accountName,
			secretKeyValue,
			timeToComplete: timeToComplete.current
		};

		const wasSuccessful = await requestPasswordReset(data);
		if (wasSuccessful) {
			navigate(links.forgotPassword.successful);
		}

		return wasSuccessful;
	};

	const info = (
		<p>
			To reset your password, please enter your username or email address in the form below. If there is an account associated with that username or email address, you will receive an email with a link authorizing a password reset.
		</p>
	);

	return (
		<NarrowContainer>
			<StandardForm
				title="Forgot password"
				onSubmit={doPasswordReset}
				information={info}
			>
				<Form.TextInput
					value={accountName}
					setValue={setAccountName}
					validators={[required('username or email address')]}
				>
					Username or email address
				</Form.TextInput>

				<Form.Honeypot
					value={secretKeyValue}
					setValue={setSecretKeyValue}
				/>
			</StandardForm>
		</NarrowContainer>
	);
}