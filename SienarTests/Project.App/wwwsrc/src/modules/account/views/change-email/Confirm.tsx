import NarrowContainer from '@/components/ui/NarrowContainer';
import { useNavigate } from 'react-router';
import { useSearchParams } from 'react-router-dom';
import { performEmailChange } from '@account/services';
import links from '@account/links';
import { useEffect } from 'react';

import type { PerformEmailChangeDto } from '@account/services';

export default function Confirm() {
	const navigate = useNavigate();
	const [query] = useSearchParams();

	const doEmailChange = async () => {
		const data: PerformEmailChangeDto = {
			verificationCode: query.get('code') ?? '',
			userId: query.get('userId') ?? ''
		};

		if (await performEmailChange(data)) {
			navigate(links.changeEmail.successful);
		}
	};

	useEffect(() => {
		(async function () {
			await doEmailChange();
		})();
	}, []);

	return (
		<NarrowContainer>
			<h1>
				Confirming
				email
			</h1>

			<p>
				We're
				confirming
				your
				new
				email
				address.
				Hold
				on...
			</p>
		</NarrowContainer>
	);
}