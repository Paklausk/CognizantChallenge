import React, {useEffect} from 'react'
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';

function SuccessNotification(props) {

	let mainTagRef = React.createRef();

	useEffect(() => {
		mainTagRef.current.classList.add('animate-in');
	}, []);

	return (
		<div ref={mainTagRef} className="animate-content animate-delay">
			<Alert severity="success" onClose={props.onClose}>
				<AlertTitle>Success</AlertTitle>
				Programming task is solved succesfully.
			</Alert>
		</div>
	);
}

export default SuccessNotification;