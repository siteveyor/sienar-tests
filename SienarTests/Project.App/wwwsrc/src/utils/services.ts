import {store} from '@/store';
import {EntityBase, ErrorDto, FilterDto, PagedDto} from '@/utils/types';
import {createErrorSnackbar, createSuccessSnackbar} from '@/utils/snackbars';

export class CrudService<T extends EntityBase> {
	protected readonly path: string;
	protected readonly name: string;

	constructor(path: string, name: string) {
		this.path = path;
		this.name = name; // TODO: capitalize?
	}

	public add = async (entity: T) => {
		const result = await sendRaw<string>(
			this.path,
			'POST',
			entity
		);

		if (result.wasSuccessful) {
			createSuccessSnackbar(`${this.name} added successfully`);
			return result.payload as string;
		}

		return '';
	}

	public edit = async (entity: T) => {
		const result = await sendRaw<null>(
			this.path,
			'PUT',
			entity
		);

		if (result.wasSuccessful) {
			createSuccessSnackbar(`${this.name} updated successfully!`);
		}

		return result.wasSuccessful;
	}

	/**
	 * Retrieve a paged list of entities based on a filter
	 *
	 * @param {FilterDto} filter The filter to use to sort and retrieve entities
	 */
	public getAll = async (filter: FilterDto) => {
		const self = this;
		const response = await sendRaw<PagedDto<T>>(
			self.path,
			'GET',
			filter
		);

		return response.wasSuccessful
			? response.payload as PagedDto<T>
			: {totalCount: 0, items: []};
	}

	/**
	 * Retrieves a single entity by ID
	 *
	 * @param {number} id The ID of the entity to retrieve
	 */
	public getById = async (id: string) => {
		const result = await sendRaw<T>(
			`${this.path}/${id}`,
			'GET'
		);

		return result.payload as T;
	}

	/**
	 * Deletes an entity by ID
	 *
	 * @param {number} id The ID of the entity to delete
	 */
	public delete = async (id: string) => {
		const result = await sendRaw<null>(
			`${this.path}/${id}`,
			'DELETE'
		);

		if (result.wasSuccessful) {
			createSuccessSnackbar(`${this.name} deleted successfully!`);
		}

		return result.wasSuccessful;
	}
}

export async function send<T>(
	uri: string,
	method: HttpMethod,
	payload: Record<string, any> | null = null,
	onSuccess: ((result: T) => Promise<void> | void) | null = null,
	errorSetters: Record<string, (errors: string[]) => void> | null = null
): Promise<boolean> {
	const result = await sendRaw<T>(uri, method, payload, errorSetters);
	if (result.wasSuccessful && onSuccess !== null) {
		await onSuccess(result.payload as T);
	}
	return result.wasSuccessful;
}

export async function sendRaw<T>(
	uri: string,
	method: HttpMethod,
	payload: Record<string, any> | null = null,
	errorSetters: Record<string, (errors: string[]) => void> | null = null
): Promise<ResponsePayload<T | ErrorMessage>> {
	const options: RequestInit = {
		method,
		credentials: 'include'
	};
	const headers: HeadersInit = {
		'RequestVerificationToken': store.getState().appData.token
	};

	if (method === 'GET' || method === 'HEAD') {
		if (payload !== null) {
			uri = `${uri}${createQueryParams(payload)}`;
		}
	} else if (payload !== null) {
		headers['Content-Type'] = 'application/json';

		// Copy payload to prevent errors in case the form needs re-submitted
		payload = Object.assign({}, payload);

		if (payload["timeToComplete"]) {
			payload["timeToComplete"] = toTimespan(Date.now() - payload["timeToComplete"]);
		}

		options.body = JSON.stringify(payload);
	}

	options.headers = headers;

	try {
		const response = await fetch(uri, options);

		const result: ResponsePayload<T> = {
			wasSuccessful: response.ok
		};

		// Sometimes, ASP.NET will send 204 AND Content-Type=application/json
		// Super annoying
		if (response.headers.get('Content-Type')?.startsWith('application/json') && response.status !== 204) {
			const json = (await response.json()) as Record<string, any>;
			if (response.ok) {
				result.payload = json as T;
			} else {
				// We had an error. Try to extract messages
				const errors = json as ErrorDto;
				let haveSetErrors = false;
				if (errors.result) {
					if (errorSetters) {
						for (let error in errors.result) {
							if (errors.result[error] && errorSetters[error]) {
								errorSetters[error](errors.result[error]);
								haveSetErrors = true;
							}
						}
					}
				}

				if (errors.message && !haveSetErrors) {
					createErrorSnackbar(errors.message);
				}
			}
		}

		return result;
	} catch (e) {
		console.error(e);
		createErrorSnackbar("We couldn't communicate with the server. Are you connected to the internet?");

		return {
			wasSuccessful: false,
			payload: {message: e instanceof Error ? e.message : 'Unknown error'}
		};
	}
}

export function createQueryParams(payload: Record<string, any>) {
	let params = '?';
	Object.keys(payload)
		.forEach(k => params = `${params}${k}=${payload[k]}&`);
	return params.substring(0, params.length - 1);
}

function toTimespan(time: number): string {
	time /= 1000;

	// If they took more than an hour to complete the form
	// we really don't care what their time was
	// so let's simplify the logic and just return an hour
	if (time > 3599) {
		return '01:00:00.0';
	}

	// Otherwise, determine how long they actually took
	// and convert it to a .NET TimeSpan
	const seconds = time % 60;
	const minutes = Math.floor(time / 60);
	return `00:${numberWithLeadingZero(minutes)}:${numberWithLeadingZero(seconds, true)}`;
}

function numberWithLeadingZero(x: number, isSeconds: boolean = false): string {
	// ASP.NET TimeSpan can parse HH:MM:SS.xxx_xxx values
	// but more "precision" than 6 decimal places will cause an error
	// so if we're formatting the seconds portion, we need to limit the decimals
	const num = isSeconds
				? x.toFixed(6)
				: x.toString(10);

	return x < 10
		   ? `0${num}`
		   : num;
}

interface ResponsePayload<T> {
	wasSuccessful: boolean,
	payload?: T
}

export interface ErrorMessage {
	message: string
}

type HttpMethod =
	'GET'
	| 'POST'
	| 'PUT'
	| 'PATCH'
	| 'DELETE'
	| 'HEAD'
	| 'OPTIONS'
	| 'CONNECT'
	| 'TRACE';