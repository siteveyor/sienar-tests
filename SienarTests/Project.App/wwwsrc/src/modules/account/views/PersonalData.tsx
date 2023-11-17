import NarrowContainer from '@/components/ui/NarrowContainer';
import StandardCard from '@/components/ui/StandardCard';
import { Button } from '@sienar/react-components';
// import {Dialog, DialogActions, DialogContent, DialogTitle} from '@mui/material';
import links from '@account/links';
import {useState} from 'react';
import { deleteAccount, DeleteAccountDto } from '@account/services';
import {useNavigate} from 'react-router';
import { Form } from '@sienar/react-components';

export default function PersonalData() {
	const navigate = useNavigate();
	const [showDialog, setShowDialog] = useState(false);
	const [password, setPassword] = useState('');

	const toggleDialog = () => setShowDialog(!showDialog);

	const doDeleteAccount = async () => {
		const data: DeleteAccountDto = { password };
		if (await deleteAccount(data)) {
			toggleDialog();
			navigate(links.deleted);
		}
	}

	const actions = (
		<>
			<Button 
				href={links.personalData.download}
				target='_blank'
			>
				Download personal data
			</Button>

			<Button
				color='error'
				onClick={toggleDialog}
			>
				Delete personal data
			</Button>
		</>
	);

	return (
		<NarrowContainer>
			<StandardCard
				title='Personal data'
				actions={actions}
			>
				<p>
					By creating an account, you give us personal data that we store. You have the right to know what data we have. You also have the right to request that we delete your personal data.
				</p>

				<p className='text--warning'>
					Deleting your personal data from our servers will also delete your account. This cannot be undone!
				</p>
			</StandardCard>

			{/*<Dialog*/}
			{/*	open={showDialog}*/}
			{/*	onClose={toggleDialog}*/}
			{/*>*/}
			{/*	<DialogTitle>Delete your data</DialogTitle>*/}
			
			{/*	<DialogContent>*/}
			{/*		<p>*/}
			{/*			Are you sure you want to delete your data? We will also have to delete your account. This cannot be undone!*/}
			{/*		</p>*/}
			
			{/*		<Form.TextInput*/}
			{/*			value={password}*/}
			{/*			setValue={setPassword}*/}
			{/*		>*/}
			{/*			Confirm your password*/}
			{/*		</Form.TextInput>*/}
			{/*	</DialogContent>*/}
			
			{/*	<DialogActions>*/}
			{/*		<Button*/}
			{/*			color='error'*/}
			{/*			onClick={doDeleteAccount}*/}
			{/*		>*/}
			{/*			Yes, delete my data*/}
			{/*		</Button>*/}
			{/*		<Button*/}
			{/*			variant='outlined'*/}
			{/*			onClick={toggleDialog}*/}
			{/*		>*/}
			{/*			No, keep my account*/}
			{/*		</Button>*/}
			{/*	</DialogActions>*/}
			{/*</Dialog>*/}
		</NarrowContainer>
	);
}