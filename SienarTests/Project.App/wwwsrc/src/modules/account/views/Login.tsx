import { Form } from '@sienar/react-components';
import NarrowContainer from '@/components/ui/NarrowContainer';
import {Button} from '@sienar/react-components';
import StandardForm from '@/components/ui/forms/StandardForm';
import {useRef, useState} from 'react';
import {required} from '@/utils/validators';
import {login, LoginDto} from '@account/services';
import {useNavigate} from 'react-router';
import {dashboard} from '@/utils/links';
import links from '@account/links';

export default function Login() {
	const navigate = useNavigate();
	const [username, setUsername] = useState('');
	const [password, setPassword] = useState('');
	const [secretKeyValue, setSecretKeyValue] = useState('');
	const [rememberMe, setRememberMe] = useState(false);
	const timeToComplete = useRef(Date.now());

	const actions = (
		<Button
			color='secondary'
			to={links.forgotPassword.index}
		>
			I forgot my password
		</Button>
	);

	const onSubmit = async () => {
		const payload: LoginDto = {
			accountName: username,
			password,
			secretKeyValue,
			rememberMe,
			timeToComplete: timeToComplete.current
		};

		const wasSuccessful = await login(payload);
		if (wasSuccessful) {
			navigate(dashboard);
		}

		return wasSuccessful;
	}

	return (
		<NarrowContainer>
			<StandardForm
				title='Login'
				onSubmit={onSubmit}
				submitText='Log in'
				additionalActions={actions}
			>
				<Form.TextInput
					validators={[required('username')]}
					value={username}
					setValue={setUsername}
				>
					Username
				</Form.TextInput>

				<Form.TextInput
					validators={[required('password')]}
					value={password}
					setValue={setPassword}
					type='password'
				>
					Password
				</Form.TextInput>

				<Form.Honeypot
					value={secretKeyValue}
					setValue={setSecretKeyValue}
				/>

				<Form.CheckboxInput
					value={rememberMe}
					setValue={setRememberMe}
				>
					Keep me signed in
				</Form.CheckboxInput>
			</StandardForm>
		</NarrowContainer>
	);
}