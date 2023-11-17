import {PropsWithChildren, ReactNode, useEffect} from 'react';
import {useNavigate} from 'react-router';
import {AuthorizeProps} from './props';
import {dashboard, unauthorized} from '@/utils/links';
import accountLinks from '@account/links';
import {useAuthorized} from '@/utils/hooks';

interface Props extends AuthorizeProps {
	mustBeLoggedOut?: boolean
}

export default function AuthorizeRoute(props: PropsWithChildren<Props>) {
	const {roles, any, mustBeLoggedOut, children} = props;
	const navigate = useNavigate();
	const isLoggedIn = useAuthorized();
	const isAuthorized = useAuthorized(roles, any);

	useEffect(() => {
		const authorized = mustBeLoggedOut ? !isLoggedIn : isAuthorized;
		const route = mustBeLoggedOut
			? dashboard
			: isLoggedIn ? unauthorized : accountLinks.login;

		if (!authorized) {
			navigate(route);
		}
	}, [isAuthorized]);

	let output: ReactNode|null;
	if (mustBeLoggedOut) {
		output = isAuthorized ? null : children;
	} else {
		output = isAuthorized ? children : null;
	}

	return output as JSX.Element|null;
}