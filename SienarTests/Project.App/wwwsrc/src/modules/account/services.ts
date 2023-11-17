import { CrudService, ErrorMessage, send, sendRaw } from '@/utils/services';
import type {VerificationCodeDto} from '@/utils/types';
import {store} from '@/store';
import {initializeSession} from '@account/store';
import { EntityBase, HoneypotDto } from '@/utils/types';
import { createErrorSnackbar, createSuccessSnackbar } from '@/utils/snackbars';

const endpoints = {
	confirm: '/sienar/account/confirm',
	email: '/sienar/account/email',
	index: '/sienar/account',
	initializeSession: '/sienar/account/initialize',
	login: '/sienar/account/login',
	password: '/sienar/account/password',
	roles: '/sienar/roles'
}

export async function register(payload: RegisterDto): Promise<boolean> {
	return send<RegisterDto>(
		endpoints.index,
		'POST',
		payload
	);
}

export async function login(payload: LoginDto): Promise<boolean> {
	const wasSuccessful = await send<SienarUserDto>(
		endpoints.login,
		'POST',
		payload,
		() => createSuccessSnackbar('Logged in successfully!')
	);

	if (!wasSuccessful) {
		return false;
	}

	return await startSession();
}

export async function startSession(): Promise<boolean> {
	return send<SessionInfo>(
		endpoints.initializeSession,
		'GET',
		null,
		payload => {
			store.dispatch(initializeSession(payload));
		}
	);
}

export async function logout(): Promise<boolean> {
	const wasSuccessful = await send<null>(
		endpoints.login,
		'DELETE',
		null,
		() => createSuccessSnackbar('Logged out successfully!')
	);

	if (!wasSuccessful) {
		return false;
	}

	return await startSession();
}

export async function requestPasswordReset(data: RequestPasswordResetDto): Promise<boolean> {
	return send<null>(
		endpoints.password,
		'DELETE',
		data
	);
}

export async function resetPassword(data: ResetPasswordDto): Promise<boolean> {
	return send<null>(
		endpoints.password,
		'PATCH',
		data
	);
}

export async function confirmAccount(data: ConfirmAccountDto): Promise<boolean> {
	return send<null>(
		endpoints.confirm,
		'POST',
		data
	);
}

export async function initiateEmailChange(data: InitiateEmailChangeDto): Promise<boolean> {
	return send<null>(
		endpoints.email,
		'POST',
		data
	);
}

export async function performEmailChange(data: PerformEmailChangeDto): Promise<boolean> {
	return send<null>(
		endpoints.email,
		'PATCH',
		data
	);
}

export async function changePassword(data: ChangePasswordDto): Promise<boolean> {
	return send<null>(
		endpoints.password,
		'POST',
		data
	);
}

export async function deleteAccount(data: DeleteAccountDto): Promise<boolean> {
	const wasSuccessful = await send<null>(
		endpoints.index,
		'DELETE',
		data
	);

	if (!wasSuccessful) {
		return false;
	}

	return await startSession();
}

export async function getAppRoles(): Promise<SienarRoleDto[]> {
	const response = await sendRaw<SienarRoleDto[]>(endpoints.roles, 'GET');
	if (!response.wasSuccessful) {
		const message = response.payload as ErrorMessage;
		createErrorSnackbar(message.message);
		return [];
	}

	return response.payload as SienarRoleDto[];
}

export interface SienarUserDto extends EntityBase {
	username: string
	email: string
	isVerified: boolean
	roles: SienarRoleDto[]
}

export interface SienarRoleDto {
	name: string
	id: string
	concurrencyStamp: string
}

export interface SessionInfo {
	token: string
	user?: SienarUserDto|null
}

export interface LoginDto extends HoneypotDto {
	accountName: string
	password: string
	rememberMe: boolean
}

export interface RequestPasswordResetDto extends HoneypotDto {
	accountName: string
}

export interface ResetPasswordDto extends HoneypotDto, VerificationCodeDto {
	userId: string
	newPassword: string
	confirmNewPassword: string
}

export interface ConfirmAccountDto extends VerificationCodeDto {
	userId: string
}

export interface RegisterDto extends HoneypotDto {
	email: string
	userName: string
	password: string
	confirmPassword: string
	acceptTos: boolean
}

export interface InitiateEmailChangeDto {
	email: string
	confirmEmail: string
	confirmPassword: string
}

export interface PerformEmailChangeDto extends VerificationCodeDto {
	userId: string
}

export interface ChangePasswordDto {
	newPassword: string
	confirmNewPassword: string
	currentPassword: string
}

export interface DeleteAccountDto {
	password: string
}

export interface AdminUserDto extends SienarUserDto {
	confirmPassword?: string
}

class AdminCrudService extends CrudService<AdminUserDto> {
	protected readonly roleAddsInProgress: Record<string, boolean> = {};
	protected readonly roleRemovalsInProgress: Record<string, boolean> = {};

	public add = async (entity: AdminUserDto) => {
		// Suuuuuuper hacky, but it's only one of several bad options because either:
		// a) we make add a proper method, in which case we run into issues where we need to constantly call .bind(), or
		// b) we make add a property that contains an arrow function, in which case we lose the ability to call super.method() because it's just a property, not a true method
		const id = await (new CrudService<AdminUserDto>(this.path, this.name)).add(entity);

		if (id) {
			await Promise.all(entity.roles.map(r => this.addToRole(id, r)));
		}

		return id;
	}

	public addToRole = async (userId: string, role: SienarRoleDto) => {
		if (this.roleAddsInProgress[role.id]) {
			return;
		}

		this.roleAddsInProgress[role.id] = true;

		const added = await send(
			`${this.path}/roles`,
			'POST',
			{ userId, roleId: role.id },
			() => createSuccessSnackbar(`User added to role ${role.name} successfully!`)
		);
		if (!added) {
			createErrorSnackbar(`Unable to add user to role ${role.name}`);
		}

		delete this.roleAddsInProgress[role.id];
		return added;
	}

	public removeFromRole = async (userId: string, role: SienarRoleDto) => {
		if (this.roleRemovalsInProgress[role.id]) {
			return;
		}

		this.roleRemovalsInProgress[role.id] = true;

		const removed = await send(
			`${this.path}/roles`,
			'DELETE',
			{ userId, roleId: role.id },
			() => createSuccessSnackbar(`User removed from role ${role.name} successfully!`)
		);
		if (!removed) {
			createErrorSnackbar(`Unable to add user to role ${role.name}`);
		}

		delete this.roleRemovalsInProgress[role.id];
		return removed;
	}
}

export const service = new AdminCrudService('/sienar/admin-account', 'user');