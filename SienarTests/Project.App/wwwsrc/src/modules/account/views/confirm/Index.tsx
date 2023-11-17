import NarrowContainer from '@/components/ui/NarrowContainer';
import {useSearchParams} from 'react-router-dom';
import {useEffect} from 'react';
import {useNavigate} from 'react-router';
import { confirmAccount } from '@account/services';
import links from '@account/links';
import type { ConfirmAccountDto } from '@account/services';

export default function Index() {
	const navigate = useNavigate();
	const [query] = useSearchParams();

	useEffect(() => {
		(async function() {
			const data: ConfirmAccountDto = {
				verificationCode: query.get('code') ?? '',
				userId: query.get('userId') ?? ''
			};
			if (await confirmAccount(data)) {
				navigate(links.confirm.successful);
			}
		})()
	}, []);

	return (
		<NarrowContainer>
			<h1>Confirming account</h1>
			<p>We are confirming your account. Hold on...</p>
		</NarrowContainer>
	)
}