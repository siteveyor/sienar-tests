import NarrowContainer from '@/components/ui/NarrowContainer';

export default function Successful() {
	return (
		<NarrowContainer>
			<h1>
				Password reset request received
			</h1>
			<p>
				Your password reset request has been processed successfully. If that account exists, you should receive a password reset request shortly. Please click the reset link in the email you receive and follow the on-screen instructions to finish resetting your password.
			</p>
		</NarrowContainer>
	);
}