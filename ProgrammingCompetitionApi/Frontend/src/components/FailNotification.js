import React, {useEffect} from 'react'
import Alert from '@mui/material/Alert';
import AlertTitle from '@mui/material/AlertTitle';

function FailNotification(props) {

	let mainTagRef = React.createRef();

	useEffect(() => {
		mainTagRef.current.classList.add('animate-in');
	}, []);

	return (
		<div ref={mainTagRef} className="animate-content animate-delay">
			<Alert severity="error" onClose={props.onClose}>
				<AlertTitle>Fail</AlertTitle>
				{props.message}
			</Alert>
		</div>
	);
}

export default FailNotification;