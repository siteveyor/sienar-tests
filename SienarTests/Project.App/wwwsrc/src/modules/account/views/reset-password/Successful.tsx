import NarrowContainer from '@/components/ui/NarrowContainer';
import { Link } from 'react-router-dom';
import links from '@account/links';

export default function Successful() {
	return (
		<NarrowContainer>
			<h1>
				Password reset successfully
			</h1>

			<p>
				Your password was reset successfully. You can now&nbsp;
				<Link to={links.login}>
					log in
				</Link>
				&nbsp;with your new password.
			</p>
		</NarrowContainer>
	);
}