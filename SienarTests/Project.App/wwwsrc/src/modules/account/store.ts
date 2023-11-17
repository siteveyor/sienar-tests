import type {SessionInfo, SienarUserDto} from '@account/services';
import {createSlice, PayloadAction} from '@reduxjs/toolkit';

export interface AppDataState {
	isLoggedIn: boolean
	token: string
	user: SienarUserDto
}

const initialState: AppDataState = {
	isLoggedIn: false,
	token: '',
	user: resetUser()
}

export const appDataSlice = createSlice({
	name: 'appData',
	initialState,
	reducers: {
		initializeSession(state, action: PayloadAction<SessionInfo>) {
			if (action.payload.user) {
				state.user = action.payload.user;
				state.isLoggedIn = true;
			} else {
				state.user = resetUser();
				state.isLoggedIn = false;
			}

			state.token = action.payload.token;
		}
	}
});

export const {initializeSession} = appDataSlice.actions;

function resetUser(): SienarUserDto {
	return {
		id: '',
		username: '',
		email: '',
		isVerified: false,
		roles: []
	};
}