import { Form } from '@sienar/react-components';
import { useEffect, useRef, useState } from 'react';
import NarrowContainer from '@/components/ui/NarrowContainer';
import {isEmail, matches, required} from '@/utils/validators';
import { Button } from '@sienar/react-components';
import links from '@account/links';
import UpsertForm from '@/components/ui/forms/UpsertForm';
import { getAppRoles, service } from '@account/services';
import { useParams } from 'react-router';
import type { AdminUserDto, SienarRoleDto } from '@account/services';

const defaultPassword = '********';

export default function Upsert() {
	const params = useParams();
	const id = useRef<string>(params['id'] ?? '');
	const isEditing = !!id.current;

	const [username, setUsername] = useState('');
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState(defaultPassword);
	const [confirmPassword, setConfirmPassword] = useState(defaultPassword);
	const [concurrencyStamp, setConcurrencyStamp] = useState('');
	const usernameInput = useRef<HTMLInputElement>(null);

	const [availableRoles, setAvailableRoles] = useState<SienarRoleDto[]>([]);
	const [selectedRoles, setSelectedRoles] = useState<SienarRoleDto[]>([]);

	useEffect(() => {
		(async () => {
			const roles = await getAppRoles();
			setAvailableRoles(roles);
		})()
	}, []);

	const toggleRole = async (role: SienarRoleDto) => {
		const index = selectedRoles.findIndex(r => r.id === role.id);
		if (index > -1) {
			if (isEditing) {
				await service.removeFromRole(id.current, role);
			}
			const newRoles = [...selectedRoles];
			newRoles.splice(index, 1);
			setSelectedRoles(newRoles);
		} else {
			if (isEditing) {
				await service.addToRole(id.current, role);
			}
			const newRoles = [...selectedRoles];
			newRoles.push(role);
			setSelectedRoles(newRoles);
		}
	};

	const mapDto = () => {
		const dto: AdminUserDto = {
			username,
			email,
			password,
			concurrencyStamp,
			roles: selectedRoles,
			isVerified: true
		};

		return dto;
	};

	const populateData = (existing: AdminUserDto) => {
		setUsername(existing.username);
		setEmail(existing.email);
		setSelectedRoles(existing.roles);
		setConcurrencyStamp(existing.concurrencyStamp!);
	};

	const reset = () => {
		setUsername('');
		setEmail('');
		setPassword(defaultPassword);
		setConfirmPassword(defaultPassword);
		setConcurrencyStamp('');
		usernameInput.current!.focus();
	};

	const addMessage = (
		<p>
			To add a new user, fill in the information in the form below. You'll need to delete the default password and enter the user's actual password. The password should contain a mix of lowercase letters, uppercase letters, numbers, and special characters, and it should be at least 8 characters long.
		</p>
	);

	const editMessage = (
		<p>
			To edit a user, fill in the information in the form below. If you don't want to edit their password, leave it as-is and no changes will be made. The password should contain a mix of lowercase letters, uppercase letters, numbers, and special characters, and it should be at least 8 characters long.
		</p>
	)

	const backButton = (
		<Button
			href={links.admin.index}
			color='secondary'
		>
			Return to users listing
		</Button>
	);

	return (
		<NarrowContainer>
			<UpsertForm
				mapDto={mapDto}
				populateData={populateData}
				service={service}
				resetForm={reset}
				addTitle='Add user'
				addSubmitText='Add user'
				editTitle='Edit user'
				editSubmitText='Update user'
				successLink={links.admin.index}
				addMessage={addMessage}
				editMessage={editMessage}
				additionalActions={backButton}
			>
				<Form.TextInput
					value={username}
					setValue={setUsername}
					validators={[required('name')]}
					inputRef={usernameInput}
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
					validators={[required('password')]}
				>
					Password
				</Form.TextInput>
				<Form.TextInput
					value={confirmPassword}
					setValue={setConfirmPassword}
					type='password'
					validators={[matches(password, 'The passwords do not match')]}
				>
					Confirm password
				</Form.TextInput>

				{/*{availableRoles?.map(r => {*/}
				{/*	const switchControl = (*/}
				{/*		<Switch*/}
				{/*			checked={selectedRoles.some(r => r.id === r.id)}*/}
				{/*			onChange={() => toggleRole(r)}*/}
				{/*		/>*/}
				{/*	);*/}
				
				{/*	return (*/}
				{/*		<FormControlLabel*/}
				{/*			key={r.id}*/}
				{/*			control={switchControl}*/}
				{/*			label={r.name}*/}
				{/*		/>*/}
				{/*	)*/}
				{/*})}*/}
			</UpsertForm>
		</NarrowContainer>
	)
}