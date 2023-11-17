import {RouteObject} from 'react-router';

export interface HoneypotDto {
	secretKeyValue: string,
	timeToComplete: number
}

export interface VerificationCodeDto {
	verificationCode: string
}

export interface FilterDto {
	searchTerm: string|null
	sortColumnName: string|null
	sortDescending: boolean
	page: number
	pageSize: number
}

export interface PagedDto<T> {
	totalCount: number
	items: T[]
}

export interface ErrorDto {
	message?: string
	result?: Record<string, string[]>
}

export interface DrawerItem {
	text: string
	icon: JSX.Element
	to?: string
	href?: string
	buttonComponent?: JSX.Element|string
	endIcon?: JSX.Element
	children?: DrawerItem[]
	authorization?: string|string[]|boolean
	any?: boolean
	priority?: number
}

export interface Module {
	router: RouteObject
	drawerItems?: DrawerItem[]
}

export type FormValueValidator<T extends unknown> = {
	(input: T): string|null
}

export type FormFieldValidator = {
	(): boolean
}

export type EntityBase = Record<string, any> & {
	id?: string
	concurrencyStamp?: string
}