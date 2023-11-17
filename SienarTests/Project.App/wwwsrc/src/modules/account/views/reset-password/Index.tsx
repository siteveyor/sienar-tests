import { Form } from '@sienar/react-components';
import NarrowContainer from '@/components/ui/NarrowContainer';
import {useNavigate} from 'react-router';
import {useSearchParams} from 'react-router-dom';
import {useRef, useState} from 'react';
import {resetPassword} from '@account/services';
import links from '@account/links';
import StandardForm from '@/components/ui/forms/StandardForm';
import {matches, required} from '@/utils/validators';
import {passwordValidators} from '@/utils/groupedValidators';
import type {ResetPasswordDto} from '@account/services';

export default function Index() {
	const navigate = useNavigate();
	const [query] = useSearchParams();
	const [newPassword, setNewPassword] = useState('');
	const [confirmNewPassword, setConfirmNewPassword] = useState('');
	const [secretKeyValue, setSecretKeyValue] = useState('');
	const timeToComplete = useRef(Date.now());

	const doPasswordReset = async () => {
		const data: ResetPasswordDto = {
			verificationCode: query.get('code') ?? '',
			newPassword,
			confirmNewPassword,
			secretKeyValue,
			timeToComplete: timeToComplete.current,
			userId: query.get('userId') ?? ''
		};

		const wasSuccessful = await resetPassword(data)
		if (wasSuccessful) {
			navigate(links.resetPassword.successful);
		}

		return wasSuccessful;
	}

	return (
		<NarrowContainer>
			<StandardForm
				title='Reset Password'
				onSubmit={doPasswordReset}
				submitText='Reset my password'
			>
				<Form.TextInput
					value={newPassword}
					setValue={setNewPassword}
					type='password'
					validators={passwordValidators('new password')}
				>
					New password
				</Form.TextInput>
				<Form.TextInput
					value={confirmNewPassword}
					setValue={setConfirmNewPassword}
					type='password'
					validators={[
						required('confirm new password'),
						matches(newPassword, 'Your passwords do not match.')
					]}
				>
					Confirm new password
				</Form.TextInput>

				<Form.Honeypot
					value={secretKeyValue}
					setValue={setSecretKeyValue}
				/>
			</StandardForm>
		</NarrowContainer>
	);
}