import NarrowContainer from '@/components/ui/NarrowContainer';
import {Link} from 'react-router-dom';
import links from '@account/links';

export default function Successful() {
	return (
		<NarrowContainer>
			<h1>Account confirmed</h1>
			<p>
				Your account has been confirmed successfully. You can now&nbsp;
				<Link to={links.login}>
					log in
				</Link>
			</p>
		</NarrowContainer>
	)
}