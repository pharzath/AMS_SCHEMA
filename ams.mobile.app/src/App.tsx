import React from 'react';
import logo from './logo.svg';
import './App.css';

import Button from '@mui/material/Button';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';

function App() {
  return (
    <div className="App">
      <AppBar position="fixed" color="primary">
        <Toolbar>
          <Typography variant="h6"> AMS Application</Typography>
        </Toolbar>
      </AppBar>

      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload. OK
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
        <Button variant="contained" color="error">
          This is my Sample
        </Button>
      </header>
    </div>
  );
}

export default App;
