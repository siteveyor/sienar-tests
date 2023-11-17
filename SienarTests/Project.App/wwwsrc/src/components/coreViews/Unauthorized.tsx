import NarrowContainer from '@/components/ui/NarrowContainer';

export default function Unauthorized() {
	return (
		<NarrowContainer>
			<h1>Unauthorized</h1>
			<p>You do not have permission to view that resource.</p>
		</NarrowContainer>
	);
}