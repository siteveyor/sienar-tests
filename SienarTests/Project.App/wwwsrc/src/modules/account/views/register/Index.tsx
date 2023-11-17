import {useRef, useState} from 'react';
import { Form } from '@sienar/react-components';
import {register} from '@account/services';
import {useNavigate} from 'react-router';
import links from '@account/links';
import NarrowContainer from '@/components/ui/NarrowContainer';
import StandardForm from '@/components/ui/forms/StandardForm';
import {isEmail, matches, maxLength, minLength, required} from '@/utils/validators';
import {passwordValidators} from '@/utils/groupedValidators';
import type {RegisterDto} from '@account/services';

export default function Index() {
	const navigate = useNavigate();

	const [email, setEmail] = useState('');
	const [username, setUsername] = useState('');
	const [password, setPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');
	const [secretKeyValue, setSecretKeyValue] = useState('');
	const [acceptTos, setAcceptTos] = useState(false);
	const timeToComplete = useRef(Date.now());

	const doRegister = async () => {
		const data: RegisterDto = {
			email,
			userName: username,
			password,
			confirmPassword,
			secretKeyValue,
			acceptTos,
			timeToComplete: timeToComplete.current
		};

		const wasSuccessful = await register(data);
		if (wasSuccessful) {
			navigate(`${links.register.successful}?username=${username}&email=${email}`);
		}

		return wasSuccessful;
	};

	const doReset = () => {
		setEmail('');
		setUsername('');
		setPassword('');
		setConfirmPassword('');
		setAcceptTos(false);
	}

	return (
		<NarrowContainer>
			<StandardForm
				title='Register'
				onSubmit={doRegister}
				onReset={doReset}
				submitText='Register'
				showReset
			>
				<Form.TextInput
					value={username}
					setValue={setUsername}
					validators={[
						required('username'),
						minLength('username', 6),
						maxLength('username', 32)
					]}
				>
					Username
				</Form.TextInput>
				<Form.TextInput
					value={email}
					setValue={setEmail}
					validators={[
						required('email'),
						isEmail('email')
					]}
				>
					Email address
				</Form.TextInput>
				<Form.TextInput
					value={password}
					setValue={setPassword}
					type='password'
					validators={passwordValidators('password')}
				>
					Password
				</Form.TextInput>
				<Form.TextInput
					value={confirmPassword}
					setValue={setConfirmPassword}
					type='password'
					validators={[
						required('confirm password'),
						matches(password, 'Your passwords do not match.')
					]}
				>
					Confirm password
				</Form.TextInput>
				<Form.CheckboxInput
					value={acceptTos}
					setValue={setAcceptTos}
					validators={[required('accept Terms of Service')]}
				>
					I accept the <a href='/privacy' target='_blank'>Terms of Service and Privacy Policy</a>
				</Form.CheckboxInput>

				<Form.Honeypot
					value={secretKeyValue}
					setValue={setSecretKeyValue}
				/>
			</StandardForm>
		</NarrowContainer>
	);
}