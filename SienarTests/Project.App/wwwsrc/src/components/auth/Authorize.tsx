import {PropsWithChildren, ReactNode} from 'react';
import {useAuthorized} from '@/utils/hooks';
import {AuthorizeProps} from './props';

interface Props extends AuthorizeProps {
	unauthorized?: ReactNode
}

export default function Authorize(props: PropsWithChildren<Props>) {
	const {roles, any, unauthorized, children} = props;

	const isAuthorized = useAuthorized(roles, any);

	return isAuthorized ? children : unauthorized;
}