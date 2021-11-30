import axios from 'axios';

function getTasksList(callback, errorCallback) {
	axios.get('/challenge/getTasksList')
	.then(function (response) {
		callback(response.data);
	})
	.catch(function (error) {
		errorCallback(error);
	});
}
function submitTask(submitData, callback, errorCallback) {
	axios.post('/challenge/submitTask', submitData)
	.then(function (response) {
		callback(response.data);
	})
	.catch(function (error) {
		errorCallback(error);
	});
}

export {getTasksList, submitTask};