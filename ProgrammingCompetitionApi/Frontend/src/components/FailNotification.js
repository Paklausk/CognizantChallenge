import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';

function FailNotification(props) {

	return (
		<Alert severity="error" onClose={props.onClose}>
			<AlertTitle>Fail</AlertTitle>
			{props.message}
		</Alert>
	);
}

export default FailNotification;