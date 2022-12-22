import './App.css';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';
import Register from './components/Register/Register';
import Description from './components/Description/Description';
import React from 'react';
import Authorization from './components/Authorization/Authorization';
import { Route, Routes } from 'react-router-dom';

function App() {
  return (
    <div className="App">
      <Header />
      <Description />
      <Routes>
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Authorization />} />
      </Routes>
      <Footer />
    </div>
  );
}

export default App;
