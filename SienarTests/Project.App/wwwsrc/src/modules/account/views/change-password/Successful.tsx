import NarrowContainer from '@/components/ui/NarrowContainer';

export default function Successful() {
	return (
		<NarrowContainer>
			<h1>
				Password changed successfully
			</h1>

			<p>
				Your password has been changed successfully. Next time you log in, you will have to use your updated password.
			</p>
		</NarrowContainer>
	);
}