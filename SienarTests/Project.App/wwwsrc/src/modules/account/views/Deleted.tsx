import NarrowContainer from '@/components/ui/NarrowContainer';
import links from '@account/links';
import { Link } from 'react-router-dom';

export default function Deleted() {
	return (
		<NarrowContainer>
			<h1>Account deleted</h1>

			<p>
				Your account has been deleted. You can't recover your personal data any longer, but you can still <Link to={links.register.index}>register</Link> any time.
			</p>

			<p>
				We're sorry to see you go. We hope to see you again soon!
			</p>
		</NarrowContainer>
	);
}