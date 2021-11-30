import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';

function SuccessNotification(props) {

	return (
		<Alert severity="success" onClose={props.onClose}>
			<AlertTitle>Success</AlertTitle>
			Programming task is solved succesfully.
		</Alert>
	);
}

export default SuccessNotification;