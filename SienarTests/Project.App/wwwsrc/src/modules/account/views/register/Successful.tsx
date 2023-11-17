import NarrowContainer from '@/components/ui/NarrowContainer';
import {useSearchParams} from 'react-router-dom';

export default function Successful() {
	const [query] = useSearchParams();

	const username = query.get('username');
	const email = query.get('email');

	return (
		<NarrowContainer>
			<h1>Registration successful</h1>
			<p>
				Welcome, {username}! Please check for an email sent to {email}. Click the link in the welcome email to verify your account. You won't be able to log in until you've verified your account.
			</p>
		</NarrowContainer>
	);
}