import { createTheme, ThemeProvider } from '@mui/material/styles';
import SubmitForm from './components/SubmitForm';
import Header from './components/Header';
import Box from '@mui/material/Box';

const theme = createTheme({
	palette: {
		text: {
			disabled: 'rgba(0, 0, 0, 0.87)',
		}
	},
});

function App(props) {
	return (
		<ThemeProvider theme={theme}>
			<div className="app">
				<Header />
				<Box sx={{ mx: "auto", my: "20px", maxWidth: 680, width: "90%" }}>
					<SubmitForm />
				</Box>
			</div>
		</ThemeProvider>
	);
}

export default App;