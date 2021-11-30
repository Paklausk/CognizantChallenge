import React, {useState, useEffect} from 'react'
import LoadingButton from '@mui/lab/LoadingButton';
import TextField from '@mui/material/TextField';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import FailNotification from './FailNotification';
import SuccessNotification from './SuccessNotification';
import {getTasksList, submitTask} from './../objects/Api'

function SubmitForm(props) {
	const [selectedTask, setSelectedTask] = useState(null);
	const [isSubmitting, setIsSubmitting] = useState(false);
	const [showSuccessNotification, setShowSuccessNotification] = useState(false);
	const [failNotificationMessage, setFailNotificationMessage] = useState(null);
	const [tasks, setTasks] = useState([]);

	let nameTagRef = React.createRef(), codeTagRef = React.createRef();

	useEffect(() => {
		getTasksList(function (tasks) {
			setTasks(tasks);
		}, function (error) {
			setFail(error.message);
		});
	}, []);
	useEffect(() => {
		codeTagRef.current.value = "";
	}, [selectedTask]);
	useEffect(() => {
		if (showSuccessNotification || failNotificationMessage)
			window.scrollTo(0,0);
	}, [showSuccessNotification, failNotificationMessage]);

	function onTaskSelect(event, selectedOption) {
		let selectedTaskId = selectedOption.props.value;
		var selectedTask = tasks.find(task => task.id == selectedTaskId);
		setSelectedTask(selectedTask);
	}
	function onSubmitClick() {
		if (!validateInputs()) return;

		setIsSubmitting(true);
		let submitData = {
			taskId: selectedTask.id,
			developersName: nameTagRef.current.value,
			code: codeTagRef.current.value,
		};
		submitTask(submitData, function (response) {
			setIsSubmitting(false);
			if (response.success)
				setSuccess();
			else setFail(response.error ?? 'Received error from server');

		}, function (error) {
			setIsSubmitting(false);
			setFail(error.message);
		});
	}
	function validateInputs() {
		if (!selectedTask) {
			setFail('No task selected');
			return false;
		}
		if (nameTagRef.current.value == '') {
			setFail('Developers name cannot be empty');
			return false;
		}
		if (codeTagRef.current.value == '') {
			setFail('Solution code cannot be empty');
			return false;
		}

		return true;
	}
	function setSuccess() {
		setShowSuccessNotification(true);
		setFailNotificationMessage(null);
	}
	function setFail(message) {
		setFailNotificationMessage(message);
		setShowSuccessNotification(false);
	}

	return (
		<div className="submitForm container">
			{ failNotificationMessage ? <FailNotification message={failNotificationMessage} onClose={() => setFailNotificationMessage(null)} /> : null }
			{ showSuccessNotification ? <SuccessNotification onClose={() => setShowSuccessNotification(false)} /> : null }
			<div className="field"><TextField id="developers-name" inputRef={nameTagRef} fullWidth margin="dense" label="Name" variant="outlined" /></div>
			<div className="field">
				<FormControl fullWidth margin="dense">
					<InputLabel id="task-label">Select task</InputLabel>
					<Select fullWidth labelId="task-label" id="task" label="Select task" value={selectedTask != null ? selectedTask.id : ''} onChange={onTaskSelect.bind(this)}>
						{tasks.map(task => <MenuItem key={task.id} value={task.id}>{task.name}</MenuItem>)}
					</Select>
				</FormControl>
			</div>
			{selectedTask != null ? <div className="field"><TextField fullWidth multiline margin="dense" disabled label="Task description" variant="outlined" value={selectedTask.description} /></div> : null}
			{selectedTask != null ? <div className="field"><TextField fullWidth multiline margin="dense" disabled label="Function header" variant="outlined" value={selectedTask.functionHeader} /></div> : null}
			<div className="field"><TextField id="programming-code" inputRef={codeTagRef} fullWidth multiline margin="dense" rows={6} label="Solution code" variant="outlined" /></div>
			{selectedTask != null ? <div className="field"><TextField fullWidth multiline margin="dense" disabled label="Function footer" variant="outlined" value={selectedTask.functionFooter} /></div> : null}
			<div className="field"><LoadingButton loading={isSubmitting} variant="contained" onClick={onSubmitClick} >Submit</LoadingButton></div>
		</div>
	);
}

export default SubmitForm;