import {configureStore} from '@reduxjs/toolkit';
import {appDataSlice} from '@account/store';

export const store = configureStore({
	reducer: {
		appData: appDataSlice.reducer
	}
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;