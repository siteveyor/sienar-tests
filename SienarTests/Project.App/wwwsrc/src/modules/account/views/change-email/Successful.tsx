import NarrowContainer from '@/components/ui/NarrowContainer';

export default function Successful() {
	return (
		<NarrowContainer>
			<h1>
				Email changed successfully
			</h1>

			<p>
				Your email has been updated successfully. All account communications will now go to your new email address, and if you use your email address to log in, you will have to use your new email address.
			</p>
		</NarrowContainer>
	);
}