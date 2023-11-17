import { Form } from '@sienar/react-components';
import NarrowContainer from '@/components/ui/NarrowContainer';
import {useNavigate} from 'react-router';
import {useState} from 'react';
import {changePassword} from '@account/services';
import links from '@account/links';
import StandardForm from '@/components/ui/forms/StandardForm';
import {passwordValidators} from '@/utils/groupedValidators';
import {matches, required} from '@/utils/validators';
import type {ChangePasswordDto} from '@account/services';

export default function Index() {
	const navigate = useNavigate();
	const [newPassword, setNewPassword] = useState('');
	const [confirmNewPassword, setConfirmNewPassword] = useState('');
	const [currentPassword, setCurrentPassword] = useState('');

	const doPasswordChange = async () => {
		const data: ChangePasswordDto = {
			newPassword,
			confirmNewPassword,
			currentPassword
		};

		const wasSuccessful = await changePassword(data);
		if (wasSuccessful) {
			navigate(links.changePassword.successful);
		}

		return wasSuccessful;
	};

	const info = (
		<p>
			Please select a new password. Your new password should be at least 8 characters long and contain a mix of lowercase and uppercase letters, numbers, and special characters.
		</p>
	);

	return (
		<NarrowContainer>
			<StandardForm
				title='Change password'
				onSubmit={doPasswordChange}
				submitText='Update password'
				information={info}
			>
				<Form.TextInput
					value={newPassword}
					setValue={setNewPassword}
					type='password'
					validators={passwordValidators('new password')}
				>
					Enter your new password
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
					Confirm your new password
				</Form.TextInput>
				<Form.TextInput
					value={currentPassword}
					setValue={setCurrentPassword}
					type='password'
					validators={[required('confirm current password')]}
				>
					Confirm your current password
				</Form.TextInput>
			</StandardForm>
		</NarrowContainer>
	);
}